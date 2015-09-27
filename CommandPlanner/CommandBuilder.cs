﻿using IwAutoUpdater.BLL.CommandPlanner.Contracts;
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

namespace IwAutoUpdater.BLL.CommandPlanner
{
    public class CommandBuilder : ICommandBuilder
    {
        private readonly IRunExternalCommand _runExternalCommand;
        private readonly IDirectory _directory;
        private readonly ILogger _logger;
        private readonly ISingleFile _singleFile;
        private readonly IHtmlGetter _htmlGetter;

        public CommandBuilder(ISingleFile singleFile, IDirectory directory, ILogger logger, IRunExternalCommand runExternalCommand, IHtmlGetter htmlGetter)
        {
            _singleFile = singleFile;
            _logger = logger;
            _directory = directory;
            _runExternalCommand = runExternalCommand;
            _htmlGetter = htmlGetter;
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
                        if (!String.IsNullOrEmpty(package.Settings.CheckUrlProxyAddress))
                        {
                            proxySettings = new ProxySettings();
                            proxySettings.Address = package.Settings.CheckUrlProxyAddress;

                            if (!String.IsNullOrEmpty(package.Settings.CheckUrlProxyPassword) && !String.IsNullOrEmpty(package.Settings.CheckUrlProxyUsername))
                            {
                                proxySettings.Username = package.Settings.CheckUrlProxyUsername;
                                proxySettings.Password = package.Settings.CheckUrlProxyPassword;
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

                // TODO: Abschließende Nachrichten verschicken (anhängen immer an finalCommand)


                // TODO: Command Kette über DoOnTrue (fortschreiten) DoOnFalse (fehlernachricht schicken) bauen

                commands.Enqueue(checkIfNewer); // mit checkIfNewer beginnt die Abarbeitungskette
            }

            return commands;
        }
    }
}
