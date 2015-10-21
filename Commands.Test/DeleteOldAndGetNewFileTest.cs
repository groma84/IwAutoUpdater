using IwAutoUpdater.CrossCutting.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.IO;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class DeleteOldAndGetNewFileTest
    {
        static string _workFolder = @"C:\zork\";
        static string _fileName = "zork.zip";
        string _fullPath = Path.Combine(_workFolder, _fileName);
        UpdatePackageAccessMock _updatePackageAccessMock;
        UpdatePackageMock _updatePackageMock;
        SingleFileMock _singleFileMock;
        CommandResult _commandResult = new CommandResult();
        private LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock();

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.Access = _updatePackageAccessMock;

            _loggerMock = new LoggerMock();
            _singleFileMock = new SingleFileMock();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _updatePackageAccessMock = null;
            _updatePackageMock = null;
            _singleFileMock = null;
        }

        [TestMethod]
        public void DeleteOldAndGetNewFileTest_Successful()
        {
            _singleFileMock.Write.Add(_fullPath, true);
            _singleFileMock.DoesExist = true;
            _singleFileMock.Delete = true;

            _updatePackageAccessMock.GetFile = new byte[] { byte.Parse("123") };
            _updatePackageAccessMock.GetFilenameOnly = _fileName;

            var gf = new DeleteOldAndGetNewFile(_workFolder, _updatePackageMock, _singleFileMock, _loggerMock);
            var actual = gf.Do();
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _singleFileMock.DeleteCalls);
            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            Assert.AreEqual(1, _singleFileMock.WriteCalls);
            Assert.AreEqual(1, _updatePackageAccessMock.GetFileCalls);
        }
    }
}
