using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.LocalFiles.Contracts
{
    public interface ISingleFile
    {
       
        DateTime GetLastModified(string fullPath);
        bool Delete(string fullPath);

        bool DoesExist(string fullPath);
        bool Write(string localFullPath, byte[] remoteFile);

        string ReadAsString(string fullPath);
    }
}
