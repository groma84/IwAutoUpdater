using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;
using System;
using IwAutoUpdater.CrossCutting.Logging.Contracts;

namespace IwAutoUpdater.BLL.Commands
{
    public class DeleteOldAndGetNewFile : Command
    {
        private readonly ILogger _logger;
        private readonly IUpdatePackage _package;
        private readonly ISingleFile _singleFile;
        private readonly string _workFolder;
        private readonly string _fullPathToLocalFile;

        public DeleteOldAndGetNewFile(string workFolder, IUpdatePackage package, ISingleFile singleFile, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;
            _singleFile = singleFile;
            _logger = logger;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());

            // DeleteOldAndGetNewFile aller Pakete soll immer zuerst ausgeführt werden,
            // damit immer erst alle Pakete heruntergeladen werden
            _vordraengelFaktor = int.MaxValue; 
        }

        public override CommandResult Do()
        {
            bool writeSuccess;
            try
            {
                _logger.Debug("Checking if {filePath} exists", _fullPathToLocalFile);
                if (_singleFile.DoesExist(_fullPathToLocalFile))
                {
                    _logger.Debug("{filePath} exists -> deleting", _fullPathToLocalFile);
                    _singleFile.Delete(_fullPathToLocalFile);
                    _logger.Debug("{filePath} deleted", _fullPathToLocalFile);

                }

                _logger.Debug("Getting remote file from {uri}", _package.PackageName);
                var remoteFile = _package.Access.GetFile();

                _logger.Debug("Writing downloaded file to {filePath}", _fullPathToLocalFile);

                writeSuccess = _singleFile.Write(_fullPathToLocalFile, remoteFile);
            }
            finally
            {
                _package.Access.Dispose();
            }
            return new CommandResult(writeSuccess);
        }

        public override Command Copy()
        {
            var x = new DeleteOldAndGetNewFile(_workFolder, _package, _singleFile, _logger);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }
    }
}