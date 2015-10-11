using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.BLL.Commands;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using IwAutoUpdater.CrossCutting.SFW.Contracts;

namespace IwAutoUpdater.BLL.CommandPlanner
{
    public class CommandBuilder : ICommandBuilder
    {
        private readonly INowGetter _nowGetter;
        private readonly IRunExternalCommand _runExternalCommand;
        private readonly IDirectory _directory;
        private readonly ILogger _logger;
        private readonly ISingleFile _singleFile;
        private readonly IHtmlGetter _htmlGetter;

        public CommandBuilder(ISingleFile singleFile, IDirectory directory, ILogger logger, IRunExternalCommand runExternalCommand, IHtmlGetter htmlGetter, INowGetter nowGetter)
        {
            _singleFile = singleFile;
            _logger = logger;
            _directory = directory;
            _runExternalCommand = runExternalCommand;
            _htmlGetter = htmlGetter;
            _nowGetter = nowGetter;
        }

        IEnumerable<Command> ICommandBuilder.GetCommands(string workFolder, IEnumerable<IUpdatePackage> updatePackages, IEnumerable<INotificationReceiver> notificationReceivers)
        {
            var commands = new Queue<Command>();

            foreach (var package in updatePackages)
            {
                _logger.Info("Queueing commands for {PackageName}", package.PackageName);

                var checkIfNewer = new CheckIfNewer(workFolder, package, _singleFile);
                var getFile = new GetFile(workFolder, package, _singleFile);
                var unzipFile = new UnzipFile(workFolder, package);
                var cleanupOldUnpackedFiles = new CleanupOldUnpackedFiles(workFolder, package, _directory);

                checkIfNewer.RunAfterCompletedWithResultTrue = getFile;
                getFile.RunAfterCompletedWithResultTrue = cleanupOldUnpackedFiles;
                cleanupOldUnpackedFiles.RunAfterCompletedWithResultTrue = unzipFile;

                Command finalCommand = unzipFile;

                if (!package.Settings.DownloadOnly)
                {
                    var runInstallerCommand = new RunInstallerCommand(package.Settings.InstallerCommand, package.Settings.InstallerCommandArguments, workFolder,
                        package, _runExternalCommand, _logger);

                    unzipFile.RunAfterCompletedWithResultTrue = runInstallerCommand;
                    finalCommand = runInstallerCommand;

                    if (!package.Settings.SkipDatabaseUpdate)
                    {
                        var updateDatabase = new UpdateDatabase(
                            package.Settings.DatabaseUpdaterCommand, package.Settings.DatabaseUpdaterCommandArguments, package.Settings.ConnectionString,
                            workFolder,
                            package, _runExternalCommand, _logger);

                        runInstallerCommand.RunAfterCompletedWithResultTrue = updateDatabase;
                        finalCommand = updateDatabase;
                    }

                    if (package.Settings.CheckUrlsAfterInstallation != null)
                    {
                        ProxySettings proxySettings = null;
                        if (package.Settings.CheckUrlProxySettings != null)
                        {
                            proxySettings = new ProxySettings();
                            proxySettings.Address = package.Settings.CheckUrlProxySettings.Address;

                            if (!String.IsNullOrEmpty(package.Settings.CheckUrlProxySettings.Password) && !String.IsNullOrEmpty(package.Settings.CheckUrlProxySettings.Username))
                            {
                                proxySettings.Username = package.Settings.CheckUrlProxySettings.Username;
                                proxySettings.Password = package.Settings.CheckUrlProxySettings.Password;
                            }
                        }

                        foreach (var url in package.Settings.CheckUrlsAfterInstallation)
                        {
                            var checkUrlHttpStatusIs200 = new CheckUrlHttpStatusIs200(url, package, _htmlGetter, proxySettings);
                            finalCommand.RunAfterCompletedWithResultTrue = checkUrlHttpStatusIs200;
                            finalCommand = checkUrlHttpStatusIs200;
                        }
                    }
                }

                // Abschließende Nachricht verschicken (anhängen immer an finalCommand)
                var notificationText = BuildNotificationText(package);
                var sendNotifications = new SendNotifications(notificationReceivers, notificationText.Subject, notificationText.Message, package);
                finalCommand.RunAfterCompletedWithResultTrue = sendNotifications;
                finalCommand = sendNotifications;

                // TODO: Command Kette über DoOnTrue (fortschreiten) DoOnFalse (fehlernachricht schicken) bauen

                commands.Enqueue(checkIfNewer); // mit checkIfNewer beginnt die Abarbeitungskette
            }

            return commands;
        }

        private class NotificationText
        {
            public string Subject;
            public string Message;
        }

        private NotificationText BuildNotificationText(IUpdatePackage package)
        {
            var shortPackageName = GetShortPackageName(package.PackageName);

            return new NotificationText()
            {
                Subject = $"Paket '{shortPackageName}' wurde am {_nowGetter.Now} aktualisiert",
                Message = $"Paket '{package.PackageName}' wurde am {_nowGetter.Now} automatisch aktualisiert"
            };
        }

        private static string GetShortPackageName(string packageName)
        {
            var splitByBackslash = packageName.Split(new[] { '\\' });
            var splitBySlash = packageName.Split(new[] { '/' });

            if (splitByBackslash.Length > 1)
            {
                return splitByBackslash.Last();
            }

            if (splitBySlash.Length > 1)
            {
                return splitBySlash.Last();
            }

            return packageName;
        }
    }
}
