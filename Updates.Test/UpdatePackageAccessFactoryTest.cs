using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IwAutoUpdater.DAL.Updates.Test
{
    [TestClass]
   public class UpdatePackageAccessFactoryTest
    {
        private IUpdatePackageAccessFactory _updatePackageAccessFactory;

        string _filePath = @"C:\banane\gelb.zip";
        string _uncPath = @"\\banane\gelb.zip";


        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

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
            var actual = _updatePackageAccessFactory.CreateLocalFileAccess(_filePath);
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void UpdatePackageAccessFactoryTest_CreateSmbAccess()
        {
            var actual = _updatePackageAccessFactory.CreateUncPathAccess(_uncPath);
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void UpdatePackageAccessFactoryTest_CreateHttpDownloadAccess()
        {
            var actual = _updatePackageAccessFactory.CreateHttpDownloadAccess(_uncPath, null, null);
            Assert.IsNotNull(actual);
        }
    }
}
