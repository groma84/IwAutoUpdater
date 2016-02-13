using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using Moq;
using SFW.Contracts;
using System.Linq;

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
        private Mock<IBlackboard> _blackboardMock;
        private GetVersionInfo _getVersionInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock();

            _updatePackageMock = new UpdatePackageMock() { Access = _updatePackageAccessMock, PackageName = _packageName };

            _singleFileMock = new SingleFileMock();
            _loggerMock = new LoggerMock();

            _blackboardMock = new Mock<IBlackboard>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void GetVersionInfoTest_FileDoesNotExist_ResultFailAndErrorMessageAndBlackboardEmpty() 
		{
            _getVersionInfo = new GetVersionInfo(_workFolder, _updatePackageMock, _fileName, _singleFileMock, _blackboardMock.Object);

            var actual = _getVersionInfo.Do();
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Successful);
            Assert.IsNull(actual.Errors.Single().Exception);
            Assert.AreEqual(@"C:\zork\version.xml does not exist", actual.Errors.Single().Text);

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            _blackboardMock.Verify(mock => mock.Add(It.IsAny<string>(), It.IsAny<BlackboardEntry>()), Times.Never);
        }                   

        [TestMethod]
        public void GetVersionInfoTest_FileExists_ResultTrueAndBlackboardFilled()
        {
            _singleFileMock.DoesExist = true;
            _singleFileMock.ReadAsString = "1.2.3";
            _getVersionInfo = new GetVersionInfo(_workFolder, _updatePackageMock, _fileName, _singleFileMock, _blackboardMock.Object);

            var actual = _getVersionInfo.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            _blackboardMock.Verify(mock => mock.Add(_packageName, It.Is<BlackboardEntry>(be => be.Content.ToString() == "Update to Version: 1.2.3")), Times.Once);
        }
    }
}
