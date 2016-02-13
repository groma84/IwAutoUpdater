using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.IO;

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

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateUncPathAccess(string uncPath)
        {
            var fi = new FileInfo(uncPath);
            return new SmbFileAccess(fi);
        }
    }
}
