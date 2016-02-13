using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.IO;

namespace IwAutoUpdater.DAL.Updates
{
    public class LocalFileAccess : IUpdatePackageAccess
    {
        private readonly string _fullFilePath;

        public LocalFileAccess(string fullFilePath)
        {
            _fullFilePath = fullFilePath;
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
            return (remoteDate > existingFileDate);
        }
      
    }
}
