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

namespace IwAutoUpdater.BLL.CommandPlanner
{
    public class CommandBuilder : ICommandBuilder
    {
        private readonly IDatabaseScript _databaseScript;
        private readonly IDirectory _directory;
        private readonly ILogger _logger;
        private readonly ISingleFile _singleFile;

        public CommandBuilder(ISingleFile singleFile, IDirectory directory, ILogger logger, IDatabaseScript databaseScript)
        {
            _singleFile = singleFile;
            _logger = logger;
            _directory = directory;
            _databaseScript = databaseScript;
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
                        package, _logger);

                    unzipFile.RunAfterCompletedWithResultTrue = runInstallerCommand;
                    finalCommand = runInstallerCommand;

                    if (!package.Settings.SkipDatabaseUpdate)
                    {
                        var updateDatabase = new UpdateInterWattDatabase(package.Settings.DatabaseUpdateConnectionString, package.Settings.DatabaseScriptSubfolder, workFolder,
                            package, _directory, _databaseScript);

                        runInstallerCommand.RunAfterCompletedWithResultTrue = updateDatabase;
                        finalCommand = updateDatabase;
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
