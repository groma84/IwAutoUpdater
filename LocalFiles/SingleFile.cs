using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System;
using System.IO;

namespace IwAutoUpdater.DAL.LocalFiles
{
    public class SingleFile : ISingleFile
    {
      
        bool ISingleFile.Delete(string fullPath)
        {
            File.Delete(fullPath);
            return true;
        }

        bool ISingleFile.DoesExist(string fullPath)
        {
            return File.Exists(fullPath);
        }

        DateTime ISingleFile.GetLastModified(string fullPath)
        {
            return File.GetLastWriteTime(fullPath);
        }

        bool ISingleFile.Write(string localFullPath, byte[] remoteFile)
        {
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(localFullPath));
            File.WriteAllBytes(localFullPath, remoteFile);

            return true;
        }

        string ISingleFile.ReadAsString(string fullPath)
        {
            return File.ReadAllText(fullPath);
        }

        void ISingleFile.Write(string localFullPath, string content)
        {
            File.WriteAllText(localFullPath, content);
        }
    }
}
