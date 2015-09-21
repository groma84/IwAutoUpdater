using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.LocalFiles
{
    public class Directory : IDirectory
    {
        void IDirectory.Delete(string fullPath)
        {
            if (System.IO.Directory.Exists(fullPath))
            {
                System.IO.Directory.Delete(fullPath, true);
            }
        }

        IEnumerable<string> IDirectory.GetFiles(string fullPath, string searchPattern)
        {
            return System.IO.Directory.EnumerateFiles(fullPath, searchPattern);
        }
    }
}
