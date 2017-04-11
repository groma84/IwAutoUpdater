using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.IO;

namespace IwAutoUpdater.DAL.Updates
{
    public class LocalFileAccess : IUpdatePackageAccess
    {
        private readonly ILogger _logger;
        private readonly string _fullFilePath;

        public LocalFileAccess(string fullFilePath, ILogger logger)
        {
            _fullFilePath = fullFilePath;
            _logger = logger;
        }

        void IDisposable.Dispose()
        {
            // NOOP, weil unsere direkten File-Zugriffe keine längerfristige Blockade auslösen
        }

        byte[] IUpdatePackageAccess.GetFile()
        {
            return File.ReadAllBytes(_fullFilePath);
        }

        string IUpdatePackageAccess.GetFilenameOnly()
        {
            return Path.GetFileName(_fullFilePath);
        }

        bool IUpdatePackageAccess.IsRemoteFileNewer(DateTime existingFileDate)
        {
            if (!File.Exists(_fullFilePath))
            {
                return false;
            }

            var remoteDate = File.GetLastWriteTime(_fullFilePath);
            _logger.Debug("SmbFileAccess -> IsRemoteFileNewer: remoteDate: {RemoteDate}", remoteDate);
            return (remoteDate > existingFileDate);
        }

    }
}
