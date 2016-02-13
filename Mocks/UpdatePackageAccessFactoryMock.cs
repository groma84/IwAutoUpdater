using IwAutoUpdater.DAL.Updates.Contracts;
using System.Collections.Generic;
using IwAutoUpdater.DAL.WebAccess.Contracts;

namespace Mocks
{
    public class UpdatePackageAccessFactoryMock : IUpdatePackageAccessFactory
    {
        public Dictionary<string, IUpdatePackageAccess> CreateLocalFileAccess = new Dictionary<string, IUpdatePackageAccess>();
        public int CreateLocalFileAccessCalled = 0;
        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateLocalFileAccess(string filePath)
        {
            ++CreateLocalFileAccessCalled;
            return CreateLocalFileAccess[filePath];
        }

        public Dictionary<string, IUpdatePackageAccess> CreateUncPathAccess = new Dictionary<string, IUpdatePackageAccess>();
        public int CreateUncPathAccessCalled = 0;
        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateUncPathAccess(string uncPath)
        {
            ++CreateUncPathAccessCalled;
            return CreateUncPathAccess[uncPath];
        }

        public Dictionary<string, IUpdatePackageAccess> CreateHttpDownloadAccess = new Dictionary<string, IUpdatePackageAccess>();
        public int CreateHttpDownloadAccessCalled = 0;
        IUpdatePackageAccess IUpdatePackageAccessFactory.CreateHttpDownloadAccess(string url, IHtmlGetter htmlGetter, ProxySettings proxySettings)
        {
            ++CreateHttpDownloadAccessCalled;
            return CreateHttpDownloadAccess[url];
        }
    }
}