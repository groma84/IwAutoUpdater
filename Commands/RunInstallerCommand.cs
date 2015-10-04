﻿using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;

namespace IwAutoUpdater.BLL.Commands
{
    public class RunInstallerCommand : Command
    {
        private readonly IRunExternalCommand _runExternalCommand;
        private readonly ILogger _logger;
        private readonly string _installerCommandArguments;
        private readonly string _installerCommand;
        private readonly string _fullPathToLocalDirectory;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public RunInstallerCommand(string installerCommand, string installerCommandArguments, string workFolder, IUpdatePackage package, IRunExternalCommand runExternalCommand, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;

            _logger = logger;
            _runExternalCommand = runExternalCommand;

            _fullPathToLocalDirectory = Path.GetFileNameWithoutExtension(Path.Combine(_workFolder, package.Access.GetFilenameOnly()));

            _installerCommand = installerCommand;
            _installerCommandArguments = installerCommandArguments;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do(CommandResult resultOfPreviousCommand)
        {
            var externalCommandResult = _runExternalCommand.Run(_installerCommand, _installerCommandArguments, _fullPathToLocalDirectory);

            _logger.Info(externalCommandResult.RecordedStandardOutput);

            if (externalCommandResult.ExitCode != 0)
            {
                return new CommandResult(false, new[] { new Error() { Text = externalCommandResult.RecordedStandardOutput, Exception = null } });
            }
            else
            {
                return new CommandResult(true);
            }
        }
    }
}