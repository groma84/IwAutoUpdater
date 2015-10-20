using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;
using System;

namespace IwAutoUpdater.BLL.Commands
{
    public class CleanupOldUnpackedFiles : Command
    {
        private readonly ILogger _logger;
        private readonly IDirectory _directory;
        private readonly string _fullPathToLocalDirectory;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public CleanupOldUnpackedFiles(string workFolder, IUpdatePackage package, IDirectory directory, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;
            _directory = directory;

            _fullPathToLocalDirectory = Path.Combine(_workFolder, Path.GetFileNameWithoutExtension(package.Access.GetFilenameOnly()));

            _logger = logger;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do()
        {
            _logger.Debug("Cleaning up folder {Foldername}", _fullPathToLocalDirectory);
            _directory.Delete(_fullPathToLocalDirectory);

            return new CommandResult(true);
        }

        public override Command Copy()
        {
            var x = new CleanupOldUnpackedFiles(_workFolder, _package, _directory, _logger);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }
    }
}
