using System;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System.IO;
using IwAutoUpdater.CrossCutting.Logging.Contracts;

namespace IwAutoUpdater.BLL.Commands
{
    public class CheckIfNewer : Command
    {
        private readonly ILogger _logger;
        private readonly Func<DateTime> _utcNowGetter;
        private readonly ISingleFile _singleFile;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;
        private readonly string _fullPathToLocalFile;

        public CheckIfNewer(string workFolder, Func<DateTime> utcNowGetter, IUpdatePackage package, ISingleFile singleFile, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;
            _singleFile = singleFile;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());

            _vordraengelFaktor = int.MaxValue; // CheckIfNewer aller Pakete soll immer mit maximaler Priorität ausgeführt werden
            _utcNowGetter = utcNowGetter;
            _logger = logger;
        }

        public override CommandResult Do()
        {
            var downloadProtocolFile = Path.Combine(_workFolder, Path.GetFileNameWithoutExtension(_fullPathToLocalFile) + ".txt");
            _logger.Debug("Checking local protocolfile at {ProtocolFile}", downloadProtocolFile);

            DateTime localDate = DateTime.MinValue;
            if (_singleFile.DoesExist(downloadProtocolFile))
            {
                var content = _singleFile.ReadAsString(downloadProtocolFile);
                if (!DateTime.TryParse(content, out localDate))
                {
                    localDate = DateTime.MinValue;
                }
            }

            _logger.Debug("Comparing remote file to local date {LocalDate}", localDate);

            var remoteIsNewer = _package.Access.IsRemoteFileNewer(localDate);

            if (remoteIsNewer)
            {
                var content = _utcNowGetter().ToString("s");
                _logger.Debug("Writing local protocolfile at {ProtocolFile} with content {Content}", downloadProtocolFile, content);
                _singleFile.Write(downloadProtocolFile, content);
            }
            return new CommandResult(remoteIsNewer);
        }

        public override Command Copy()
        {
            var x = new CheckIfNewer(_workFolder, _utcNowGetter, _package, _singleFile, _logger);
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