using System;
using System.Collections.Generic;
using SimpleInjector;
using System.Reflection;
using System.IO;
using System.Linq;
using IwAutoUpdater.CrossCutting.Base;

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

            InitDIMappingsViaReflection(container);

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

            contractAssemblies = new Queue<Assembly>();
            implementationAssemblies = new Queue<Assembly>();
            foreach (var dllFilePath in Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories))
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
