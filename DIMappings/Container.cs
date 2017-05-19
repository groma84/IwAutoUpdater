using System;
using System.Collections.Generic;
using SimpleInjector;
using System.Reflection;
using System.IO;
using System.Linq;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.BLL.AutoUpdater;
using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.BLL.CommandPlanner;
using IwAutoUpdater.CrossCutting.Configuration;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.CrossCutting.Logging;
using IWAutoUpdater.CrossCutting.SFW.Contracts;
using IWAutoUpdater.CrossCutting.SFW;
using IwAutoUpdater.DAL.EMails.Contracts;
using IwAutoUpdater.DAL.EMails;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.ExternalCommands;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.LocalFiles;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Notifications;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.Updates;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using IwAutoUpdater.DAL.WebAccess;

namespace IwAutoUpdater.DIMappings
{
    public static class Container
    {
        /// <summary>
        /// <para>Container mit Standardregistrierungen (ermittelt via Reflection) füllen.</para>
        /// AdditionalRegistrations können anschließend bestehende Registrierungen überschreiben.
        /// Beispiel: Für Tests.
        /// </summary>
        /// <param name="additionalRegistrations"></param>
        /// <returns></returns>
        public static SimpleInjector.Container Init(IEnumerable<Action<SimpleInjector.Container>> additionalRegistrations)
        {
            return InitCore(additionalRegistrations);
        }

        /// <summary>
        /// Container mit Standardregistrierungen (ermittelt via Reflection) füllen.
        /// </summary>
        /// <returns></returns>
        public static SimpleInjector.Container Init()
        {
            return InitCore();
        }

        private static SimpleInjector.Container InitCore(IEnumerable<Action<SimpleInjector.Container>> additionalRegistrations = null)
        {
            SimpleInjector.Container container = new SimpleInjector.Container();


            /** da wir nicht zuviele Interfaces haben, machen wir es alles hier an einer Stelle
            /* vorher war es schön bequem via Reflection&T4-Template, leider erkennt VS dann beim
            /* Debuggen nicht mehr, welche DLLs er laden muss...
            */

            // CrossCutting.Configuration
            container.Register(typeof(IConfiguration), typeof(JsonFileConfiguration), Lifestyle.Singleton);
            container.Register(typeof(IConfigurationFileAccess), typeof(ConfigurationFileAccess), Lifestyle.Singleton);
            // CrossCutting.Logging
            container.Register(typeof(ILogger), typeof(Logger), Lifestyle.Singleton);
            // CrossCutting.SFW
            container.Register(typeof(IBlackboard), typeof(Blackboard), Lifestyle.Singleton);
            container.Register(typeof(INowGetter), typeof(NowGetter), Lifestyle.Singleton);

            // DAL.EMails
            container.Register(typeof(ISendMail), typeof(SendMail), Lifestyle.Singleton);
            // DAL.ExternalCommands
            container.Register(typeof(IRunExternalCommand), typeof(ExternalCommandRunner), Lifestyle.Singleton);
            // DAL.LocalFiles
            container.Register(typeof(IDirectory), typeof(DAL.LocalFiles.Directory), Lifestyle.Singleton);
            container.Register(typeof(ISingleFile), typeof(SingleFile), Lifestyle.Singleton);
            // DAL.Notifications
            container.Register(typeof(INotificationReceiverFactory), typeof(NotificationReceiverFactory), Lifestyle.Singleton);
            // DAL.Updates
            container.Register(typeof(IUpdatePackageAccessFactory), typeof(UpdatePackageAccessFactory), Lifestyle.Singleton);
            container.Register(typeof(IUpdatePackageFactory), typeof(UpdatePackageFactory), Lifestyle.Singleton);
            // DAL.WebAccess
            container.Register(typeof(IHtmlGetter), typeof(HtmlGetter), Lifestyle.Singleton);

            // BLL.AutoUpdater
            container.Register(typeof(IAutoUpdaterCommandCreator), typeof(AutoUpdaterCommandCreator), Lifestyle.Singleton);
            container.Register(typeof(IAutoUpdaterThreadFactory), typeof(AutoUpdaterThreadFactory), Lifestyle.Singleton);
            container.Register(typeof(IAutoUpdaterWorker), typeof(AutoUpdaterWorker), Lifestyle.Singleton);
            // BLL.CommandPlanner
            container.Register(typeof(ICheckTimer), typeof(CheckTimer), Lifestyle.Singleton);
            container.Register(typeof(ICommandBuilder), typeof(CommandBuilder), Lifestyle.Singleton);
            container.Register(typeof(IConfigurationConverter), typeof(ConfigurationConverter), Lifestyle.Singleton);


            if (additionalRegistrations != null)
            {
                container.Options.AllowOverridingRegistrations = true;

                foreach (var additionalRegistration in additionalRegistrations)
                {
                    additionalRegistration(container);
                }
            }

            container.Verify();

            return container;
        }

        static void Register(SimpleInjector.Container container, Type iface, Type impl, Lifestyle lifestyle)
        {

        }

        private static void InitDIMappingsViaReflection(SimpleInjector.Container container)
        {
            //Idee: Wir laden alle DLLs aus unserem Ordner, deren Dateiname mit IwAutoUpdater startet und nicht mit .Test endet
            //Anschließend gehen wir durch alle .Contracts-DLLs durch und suchen uns die Public Interfaces
            //Dann gehen wir durch die Implementierungs-DLLs durch und suchen für jede Klasse durch, ob deren Interfaces in den Public Interfaces der Contracts sind. Falls ja -> Registrieren

            Queue<Assembly> contractAssemblies, implementationAssemblies;
            LadeDlls(out contractAssemblies, out implementationAssemblies);

            HashSet<Type> interfaces = ErmittleZuImplementierendeInterfaces(contractAssemblies);

            RegistriereImplementierungenZuInterfaces(container, implementationAssemblies, interfaces);
        }

        private static void RegistriereImplementierungenZuInterfaces(SimpleInjector.Container container, Queue<Assembly> implementationAssemblies, HashSet<Type> interfaces)
        {
            var interfaceByNameComparer = new CrossCutting.SFW.EqualityComparerDefault<Type>((left, right) => left.Name == right.Name);
            foreach (var impl in implementationAssemblies)
            {
                var exportedTypesWithInterfaces = impl.GetExportedTypes().Where(a => a.GetInterfaces().Any());
                foreach (var etwi in exportedTypesWithInterfaces)
                {
                    foreach (var iface in etwi.GetInterfaces().Where(i => interfaces.Contains(i, interfaceByNameComparer)))
                    {
                        Lifestyle registerAsLifestyle = null;
                        if (iface.IsDefined(typeof(DIAsTransientAttribute)))
                        {
                            registerAsLifestyle = Lifestyle.Transient;
                        }
                        else if (iface.IsDefined(typeof(DIAsSingletonAttribute)))
                        {
                            registerAsLifestyle = Lifestyle.Singleton;
                        }
                        else
                        {
                            // unser Standardverhalten, wenn gar nichts angegeben ist, ist "Singleton",
                            // weil das bei meiner Implementierung 95% der Fälle abdeckt
                            registerAsLifestyle = Lifestyle.Singleton;
                        }
                        container.Register(iface, etwi, registerAsLifestyle);
                    }
                }
            }
        }

        private static HashSet<Type> ErmittleZuImplementierendeInterfaces(Queue<Assembly> contractAssemblies)
        {
            // Alle Interfaces suchen, die kein Blacklist-Flag für "nicht via DI verfügbar sein" haben
            return new HashSet<Type>(contractAssemblies
                                                .SelectMany(a => a.GetExportedTypes()
                                                                    .Where(t => t.IsInterface && !t.IsDefined(typeof(DIIgnoreAttribute)))
                                                                    )
                                                            );
        }

        private static void LadeDlls(out Queue<Assembly> contractAssemblies, out Queue<Assembly> implementationAssemblies)
        {
            Uri assemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            string path = Path.GetDirectoryName(assemblyUri.LocalPath);

            Console.WriteLine($"LadeDlls, Pfad: {path}");


            contractAssemblies = new Queue<Assembly>();
            implementationAssemblies = new Queue<Assembly>();
            foreach (var dllFilePath in System.IO.Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly))
            {
                if (dllFilePath.ToLowerInvariant().EndsWith(".test.dll") || dllFilePath.ToLowerInvariant().Contains("mock"))
                {
                    continue;
                }
                else if (dllFilePath.ToLowerInvariant().Contains("contracts"))
                {
                    contractAssemblies.Enqueue(Assembly.Load(File.ReadAllBytes(dllFilePath)));
                }
                else if (Path.GetFileName(dllFilePath.ToLowerInvariant()).StartsWith("iwautoupdater"))
                {
                    implementationAssemblies.Enqueue(Assembly.Load(File.ReadAllBytes(dllFilePath)));
                }
            }
        }
    }
}
