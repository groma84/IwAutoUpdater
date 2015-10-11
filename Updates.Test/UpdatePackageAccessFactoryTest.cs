using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [ExpectedException(typeof(Exception))]
        public void UpdatePackageAccessFactoryTest_CreateSmbAccess()
        {
            // wir erwarten eine Exception, weil es den Pfad gar nicht gibt
            var actual = _updatePackageAccessFactory.CreateUncPathAccess(_uncPath, String.Empty, String.Empty);
            Assert.IsNotNull(actual);
        }
    }
}
