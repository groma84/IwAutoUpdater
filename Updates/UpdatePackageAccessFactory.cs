using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Updates
{
    public class UpdatePackageAccessFactory : IUpdatePackageAccessFactory
    {
        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateLocalFileAccess(string filePath)
        {
            return new LocalFileAccess(filePath);
        }

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateHttpDownloadAccess(string url)
        {
            throw new NotImplementedException();
        }

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateUncPathAccess(string uncPath, string username, string password)
        {
            var fi = new FileInfo(uncPath);
            return new SmbFileAccess(fi, username, password);
        }
    }
}
