using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Updates
{
    public class LocalFileAccess : IUpdatePackageAccess
    {
        private readonly string _fullFilePath;

        public LocalFileAccess(string fullFilePath)
        {
            _fullFilePath = fullFilePath;
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
