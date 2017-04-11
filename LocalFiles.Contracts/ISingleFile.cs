using System;

namespace IwAutoUpdater.DAL.LocalFiles.Contracts
{
    public interface ISingleFile
    {
        DateTime GetLastModified(string fullPath);
        bool Delete(string fullPath);

        bool DoesExist(string fullPath);
        bool Write(string localFullPath, byte[] remoteFile);
        void Write(string localFullPath, string content);

        string ReadAsString(string fullPath);
    }
}
