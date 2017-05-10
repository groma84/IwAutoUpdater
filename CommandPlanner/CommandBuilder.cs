﻿using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.BLL.Commands;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using IwAutoUpdater.CrossCutting.SFW.Contracts;
using SFW.Contracts;

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
        readonly IBlackboard _blackboard;

        public CommandBuilder(ISingleFile singleFile, IDirectory directory, ILogger logger,
            IRunExternalCommand runExternalCommand, IHtmlGetter htmlGetter, INowGetter nowGetter,
            IBlackboard blackboard)
        {
            _blackboard = blackboard;
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
                _logger.Debug("Building commands for {PackageName}", package.PackageName);

                var checkIfNewer = new CheckIfNewer(workFolder, () => DateTime.Now, package, _singleFile, _logger);
                var getFile = new DeleteOldAndGetNewFile(workFolder, package, _singleFile, _logger);
                var unzipFile = new UnzipFile(workFolder, package.Settings.ZipPassword, package, _directory, _logger);
                var checkFreeDiskspace = new CheckFreeDiskspace(workFolder, package);
                var cleanupOldUnpackedFiles = new CleanupOldFiles(false, workFolder, package, _directory, _singleFile, _logger);

                checkIfNewer.RunAfterCompletedWithResultTrue = getFile;

                getFile.RunAfterCompletedWithResultTrue = cleanupOldUnpackedFiles;
                getFile.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, getFile, _blackboard);

                Command finalCommand = cleanupOldUnpackedFiles;

                if (!package.Settings.DownloadOnly)
                {
                    cleanupOldUnpackedFiles.RunAfterCompletedWithResultTrue = checkFreeDiskspace;
                    cleanupOldUnpackedFiles.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, cleanupOldUnpackedFiles, _blackboard);

                    finalCommand = checkFreeDiskspace;

                    checkFreeDiskspace.RunAfterCompletedWithResultTrue = unzipFile;
                    checkFreeDiskspace.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, checkFreeDiskspace, _blackboard);

                    finalCommand = unzipFile;

                    var runInstallerCommand = new RunInstallerCommand(package.Settings.InstallerCommand, package.Settings.InstallerCommandArguments, workFolder,
                        package, _runExternalCommand, _logger);

                    unzipFile.RunAfterCompletedWithResultTrue = runInstallerCommand;
                    unzipFile.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, unzipFile, _blackboard);

                    finalCommand = runInstallerCommand;

                    Command updateDatabase = null;
                    if (!package.Settings.SkipDatabaseUpdate)
                    {
                        updateDatabase = new UpdateDatabase(
                            package.Settings.DatabaseUpdaterCommand, package.Settings.DatabaseUpdaterCommandArguments, package.Settings.ConnectionString,
                            workFolder,
                            package, _runExternalCommand, _logger);

                        runInstallerCommand.RunAfterCompletedWithResultTrue = updateDatabase;
                        runInstallerCommand.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, runInstallerCommand, _blackboard);

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
                            var checkUrlHttpStatusIs200 = new CheckUrlHttpStatusIs200(url, package, _htmlGetter, _logger, proxySettings);
                            finalCommand.RunAfterCompletedWithResultTrue = checkUrlHttpStatusIs200;
                            finalCommand.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, finalCommand.Copy(), _blackboard);
                            finalCommand = checkUrlHttpStatusIs200;
                        }
                    }

                    // ggf. Versionsinfo Datei auslesen
                    if (!string.IsNullOrEmpty(package.Settings.ReadVersionInfoFrom))
                    {
                        var getVersionInfo = new GetVersionInfo(workFolder, package, package.Settings.ReadVersionInfoFrom, _singleFile, _blackboard);
                        finalCommand.RunAfterCompletedWithResultTrue = getVersionInfo;
                        finalCommand.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, finalCommand.Copy(), _blackboard);
                        finalCommand = getVersionInfo;
                    }

                    // Entpacktes Verzeichnis & lokale ZIP-Datei löschen (anhängen immer an finalCommand)
                    var cleanupOldAfterDeployment = new CleanupOldFiles(!package.Settings.DownloadOnly, workFolder, package, _directory, _singleFile, _logger);
                    finalCommand.RunAfterCompletedWithResultTrue = cleanupOldAfterDeployment;
                    finalCommand.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, finalCommand.Copy(), _blackboard);
                    finalCommand = cleanupOldAfterDeployment;
                }

                // Abschließende Nachricht verschicken (anhängen immer an finalCommand)
                var sendNotifications = new SendNotifications(notificationReceivers, package.Settings.DownloadOnly, package.Settings.SkipDatabaseUpdate, package, _nowGetter, _blackboard);
                finalCommand.RunAfterCompletedWithResultTrue = sendNotifications;
                finalCommand.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, finalCommand.Copy(), _blackboard);
                finalCommand = sendNotifications;

                // Blackboard aufräumen für unser package
                var cleanupBlackboard = new CleanupBlackboard(package, _blackboard);
                finalCommand.RunAfterCompletedWithResultTrue = cleanupBlackboard;
                finalCommand.RunAfterCompletedWithResultFalse = new SendErrorNotifications(notificationReceivers, finalCommand.Copy(), _blackboard);
                finalCommand = cleanupBlackboard;

                commands.Enqueue(checkIfNewer); // mit checkIfNewer beginnt die Abarbeitungskette
            }

            return commands;
        }
    }
}
