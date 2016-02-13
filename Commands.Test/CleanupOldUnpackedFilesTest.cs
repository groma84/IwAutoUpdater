using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Mocks;
using IwAutoUpdater.CrossCutting.Base;
using System.IO;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CleanupOldUnpackedFilesTest 
    {
        private CleanupOldUnpackedFiles _cleanupOldUnpackedFilesTest;
        private string _workFolder = "CleanupOldUnpackedFilesTestDirectory";
        private string _getFileNameOnly = "cleanFilesTest";
        private UpdatePackageMock _updatePackageMock;
        private UpdatePackageAccessMock _updatePackageAccessMock;
        private DirectoryMock _directoryMock;

        CommandResult _commandResult;
        private LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _commandResult = new CommandResult();

            _updatePackageAccessMock = new UpdatePackageAccessMock();
            _updatePackageAccessMock.GetFilenameOnly = _getFileNameOnly; // wird sowieso nie echt drauf zugegriffen
            _updatePackageMock = new UpdatePackageMock() { Access = _updatePackageAccessMock };

            _directoryMock = new DirectoryMock();

            _loggerMock = new LoggerMock();

            _cleanupOldUnpackedFilesTest = new CleanupOldUnpackedFiles(_workFolder, _updatePackageMock, _directoryMock, _loggerMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _cleanupOldUnpackedFilesTest = null;

            _commandResult = null;
            _updatePackageMock = null;
            _directoryMock = null;
            _loggerMock = null;

        }

        [TestMethod]
        public void CleanupOldUnpackedFilesTest_Do_DeleteExistingDirectoryCalledAndReturnTrue()
        {
            var actual = _cleanupOldUnpackedFilesTest.Do();
            Assert.AreEqual(1, _directoryMock.DeleteCalled);
            Assert.AreEqual(Path.Combine(_workFolder, _getFileNameOnly), _directoryMock.LastDeletedPath);

            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());
        }
    }

}
