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
    public class LocalFileAccessTest
    {
        IUpdatePackageAccess _localFileAccess;
        string _filePath = "LocalFileAccessTest.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _localFileAccess = new LocalFileAccess(_filePath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _localFileAccess = null;
        }

        [TestMethod]
        public void LocalFileAccessTest_SmokeTest()
        {
            Assert.AreEqual(_filePath, _localFileAccess.GetFilenameOnly());
            Assert.IsTrue(_localFileAccess.IsRemoteFileNewer(DateTime.Now.AddMonths(-1)));
            Assert.AreEqual(new byte[] { byte.Parse("239") }.First(), _localFileAccess.GetFile().First());
        }
    }
}
