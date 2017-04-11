using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.IO;

namespace IwAutoUpdater.DAL.Updates
{
    public class SmbFileAccess : IUpdatePackageAccess
    {
        private readonly ILogger _logger;
        FileInfo _fileInfo;

        public SmbFileAccess(FileInfo fileInfo, ILogger logger)
        {
            _fileInfo = fileInfo;
            _logger = logger;
        }

        void IDisposable.Dispose()
        {
        }

        byte[] IUpdatePackageAccess.GetFile()
        {
            return File.ReadAllBytes(_fileInfo.FullName);
        }

        string IUpdatePackageAccess.GetFilenameOnly()
        {
            return _fileInfo.Name;
        }

        bool IUpdatePackageAccess.IsRemoteFileNewer(DateTime existingFileDate)
        {
            // GetLastModified gibt DateTime.MinValue zurück, falls die Datei gar nicht existiert 
            // -> dann kommt korrekterweise hier immer false raus
            _fileInfo.Refresh();
            if (!_fileInfo.Exists)
            {
                return false;
            }

            var remoteDate = _fileInfo.LastWriteTime;
            _logger.Debug("SmbFileAccess -> IsRemoteFileNewer: remoteDate: {RemoteDate}", remoteDate);
            return (remoteDate > existingFileDate);
        }
    }
}
