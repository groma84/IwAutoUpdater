using System.Collections.Generic;

namespace IwAutoUpdater.DAL.LocalFiles.Contracts
{
    public interface IDirectory
    {
        void Delete(string fullPath);
        IEnumerable<string> GetFiles(string fullPath, string searchPattern);
    }
}