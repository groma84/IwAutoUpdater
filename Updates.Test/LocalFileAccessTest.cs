using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Linq;

namespace IwAutoUpdater.DAL.Updates.Test
{
    [TestClass]
    public class LocalFileAccessTest
    {
        IUpdatePackageAccess _localFileAccess;
        string _filePath = "LocalFileAccessTest.txt";
        private ILogger _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _loggerMock = new LoggerMock();

            _localFileAccess = new LocalFileAccess(_filePath, _loggerMock);
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
            Assert.IsTrue(_localFileAccess.IsRemoteFileNewer(new DateTime(2015, 05, 15)));
            Assert.AreEqual(new byte[] { byte.Parse("239") }.First(), _localFileAccess.GetFile().First());
        }
    }
}
