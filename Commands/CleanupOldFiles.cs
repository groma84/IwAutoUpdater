using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;
using System;

namespace IwAutoUpdater.BLL.Commands
{
    public class CleanupOldFiles : Command
    {
        private readonly bool _deleteZipFile;
        private readonly ISingleFile _singleFile;
        private readonly ILogger _logger;
        private readonly IDirectory _directory;
        private readonly string _fullPathToLocalDirectory;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;
        private readonly string _fullPathToZip;

        public CleanupOldFiles(bool deleteZipFile, string workFolder, IUpdatePackage package, IDirectory directory, ISingleFile singleFile, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;
            _directory = directory;

            _fullPathToLocalDirectory = Path.Combine(_workFolder, Path.GetFileNameWithoutExtension(package.Access.GetFilenameOnly()));
            _fullPathToZip = Path.Combine(_workFolder, package.Access.GetFilenameOnly());

            _logger = logger;
            _singleFile = singleFile;
            _deleteZipFile = deleteZipFile;
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
            try
            {
                _logger.Debug("Cleaning up folder {Foldername}", _fullPathToLocalDirectory);
                _directory.Delete(_fullPathToLocalDirectory);

                if (_deleteZipFile)
                {
                    _logger.Debug("Removing downloaded zip {Zip}", _fullPathToZip);
                    _singleFile.Delete(_fullPathToZip);
                }

                return new CommandResult(true);
            }
            catch (Exception ex)
            {
                return new CommandResult(false, new[] { new Error { Exception = ex } });
            }
        }

        public override Command Copy()
        {
            var x = new CleanupOldFiles(_deleteZipFile, _workFolder, _package, _directory, _singleFile, _logger);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }
    }
}
