using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
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

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateHttpDownloadAccess(string url, IHtmlGetter htmlGetter, ProxySettings proxySettings)
        {
            return new HttpDownloadAccess(url, htmlGetter, proxySettings);
        }

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateUncPathAccess(string uncPath)
        {
            var fi = new FileInfo(uncPath);
            return new SmbFileAccess(fi);
        }
    }
}
