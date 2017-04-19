using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace IwAutoUpdater.DAL.Updates.Test
{
    [TestClass]
   public class UpdatePackageAccessFactoryTest
    {
        private IUpdatePackageAccessFactory _updatePackageAccessFactory;

        string _filePath = @"C:\banane\gelb.zip";
        string _uncPath = @"\\banane\gelb.zip";
        private ILogger _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _loggerMock = new LoggerMock();

            _updatePackageAccessFactory = new UpdatePackageAccessFactory();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _updatePackageAccessFactory = null;
        }

        [TestMethod]
        public void UpdatePackageAccessFactoryTest_CreateLocalFile()
        {
            var actual = _updatePackageAccessFactory.CreateLocalFileAccess(_filePath, _loggerMock);
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void UpdatePackageAccessFactoryTest_CreateSmbAccess()
        {
            var actual = _updatePackageAccessFactory.CreateUncPathAccess(_uncPath, _loggerMock);
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void UpdatePackageAccessFactoryTest_CreateHttpDownloadAccess()
        {
            var actual = _updatePackageAccessFactory.CreateHttpDownloadAccess(_uncPath, null, null, null, _loggerMock,  null);
            Assert.IsNotNull(actual);
        }
    }
}
