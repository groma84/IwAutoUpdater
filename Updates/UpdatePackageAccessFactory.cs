using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using System.IO;

namespace IwAutoUpdater.DAL.Updates
{
    public class UpdatePackageAccessFactory : IUpdatePackageAccessFactory
    {
        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateLocalFileAccess(string filePath, ILogger logger)
        {
            return new LocalFileAccess(filePath, logger);
        }

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateHttpDownloadAccess(string url, IHtmlGetter htmlGetter, ILogger logger, ProxySettings proxySettings)
        {
            return new HttpDownloadAccess(url, htmlGetter, logger, proxySettings);
        }

        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateUncPathAccess(string uncPath, ILogger logger)
        {
            var fi = new FileInfo(uncPath);
            return new SmbFileAccess(fi, logger);
        }
    }
}
