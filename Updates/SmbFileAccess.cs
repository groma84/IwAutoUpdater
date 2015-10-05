using IngSoft.SmbClient;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Updates
{
    public class SmbFileAccess : IUpdatePackageAccess
    {
        FileInfo _fileInfo;

        SmbClient _smbClient;

        public SmbFileAccess(FileInfo fileInfo, string username, string password)
        {
            _fileInfo = fileInfo;
            NetworkCredential credentials = new NetworkCredential(username, password);         
            _smbClient = new SmbClient(credentials, new Uri(fileInfo.DirectoryName));
        }

        void IDisposable.Dispose()
        {
            ((IDisposable)_smbClient).Dispose();
        }

        byte[] IUpdatePackageAccess.GetFile()
        {
            return _smbClient.GetFileAsByteArray(_fileInfo);
        }

        string IUpdatePackageAccess.GetFilenameOnly()
        {
            return _fileInfo.Name;
        }

        bool IUpdatePackageAccess.IsRemoteFileNewer(DateTime existingFileDate)
        {
            // GetLastModified gibt DateTime.MinValue zurück, falls die Datei gar nicht existiert 
            // -> dann kommt korrekterweise hier immer false raus
            var remoteDate = _smbClient.GetLastModified(_fileInfo);
            return (remoteDate > existingFileDate);
        }
    }
}
