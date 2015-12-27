using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class GetVersionInfoTest
    {
        private UpdatePackageAccessMock _updatePackageAccessMock;
        private UpdatePackageMock _updatePackageMock;
        private SingleFileMock _singleFileMock;
        static string _workFolder = @"C:\zork\";
        static string _packageName = @"GetVersionInfoTest";
        static string _fileName = @"version.xml";
        private LoggerMock _loggerMock;
        private BlackboardMock _blackboardMock;
        private GetVersionInfo _getVersionInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock();

            _updatePackageMock = new UpdatePackageMock() { Access = _updatePackageAccessMock, PackageName = _packageName };

            _singleFileMock = new SingleFileMock();
            _loggerMock = new LoggerMock();

            _blackboardMock = new BlackboardMock();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void GetVersionInfoTest_FileDoesNotExist_ResultFailAndErrorMessageAndBlackboardEmpty() 
		{
            _getVersionInfo = new GetVersionInfo(_workFolder, _updatePackageMock, _fileName, _singleFileMock, _blackboardMock);

            var actual = _getVersionInfo.Do();
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Successful);
            Assert.IsNull(actual.Errors.Single().Exception);
            Assert.AreEqual(@"C:\zork\version.xml does not exist", actual.Errors.Single().Text);

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            Assert.AreEqual(0, _blackboardMock.AddCalls);
            Assert.AreEqual(0, _blackboardMock.Added.Count);
        }                   

        [TestMethod]
        public void GetVersionInfoTest_FileExists_ResultTrueAndBlackboardFilled()
        {
            _singleFileMock.DoesExist = true;
            _singleFileMock.ReadAsString = "1.2.3";
            _getVersionInfo = new GetVersionInfo(_workFolder, _updatePackageMock, _fileName, _singleFileMock, _blackboardMock);

            var actual = _getVersionInfo.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            Assert.AreEqual(1, _blackboardMock.AddCalls);
            Assert.AreEqual(1, _blackboardMock.Added.Count);
            Assert.AreEqual(_packageName, _blackboardMock.Added.First().Key);
            Assert.AreEqual(1, _blackboardMock.Added.First().Value.Count);
            Assert.AreEqual("Update to Version: 1.2.3", _blackboardMock.Added.First().Value.First().Content);
        }
    }
}
