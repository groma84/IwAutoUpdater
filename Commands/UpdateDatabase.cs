using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;

namespace IwAutoUpdater.BLL.Commands
{
    public class UpdateDatabase : Command
    {
        private readonly IRunExternalCommand _runExternalCommand;
        private readonly ILogger _logger;
        private readonly string _databaseUpdaterArguments;
        private readonly string _datebaseUpdaterCommand;
        private readonly string _connectionString;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;
        private readonly string _fullPathToLocalDirectory;

        public UpdateDatabase(string datebaseUpdaterCommand, string databaseUpdaterArguments, string connectionString, string workFolder, IUpdatePackage package, IRunExternalCommand runExternalCommand, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;

            _fullPathToLocalDirectory = Path.Combine(_workFolder, Path.GetFileNameWithoutExtension(package.Access.GetFilenameOnly()));

            _connectionString = connectionString;
            _datebaseUpdaterCommand = datebaseUpdaterCommand;
            _databaseUpdaterArguments = databaseUpdaterArguments;
            _logger = logger;
            _runExternalCommand = runExternalCommand;
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
            var argumentsWithConnectionString = InsertConnectionString(_databaseUpdaterArguments, _connectionString);

            var externalCommandResult = _runExternalCommand.Run(_datebaseUpdaterCommand, argumentsWithConnectionString, _fullPathToLocalDirectory);

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

        private string InsertConnectionString(string databaseUpdaterArguments, string connectionString)
        {
            return databaseUpdaterArguments.ReplaceIgnoreCase("<<connectionString>>", connectionString);
        }
    }
}
