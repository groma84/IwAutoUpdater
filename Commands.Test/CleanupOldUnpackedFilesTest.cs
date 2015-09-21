using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using Mocks;
using IwAutoUpdater.CrossCutting.Base;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CleanupOldUnpackedFilesTest 
    {
        private CleanupOldUnpackedFiles _cleanupOldUnpackedFilesTest;
        private string _workFolder = "CleanupOldUnpackedFilesTestDirectory";
        private UpdatePackageMock _updatePackageMock;
        private UpdatePackageAccessMock _updatePackageAccessMock;
        private DirectoryMock _directoryMock;

        CommandResult _commandResult;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _commandResult = new CommandResult();

            _updatePackageAccessMock = new UpdatePackageAccessMock();
            _updatePackageAccessMock.GetFilenameOnly = _workFolder; // wird sowieso nie echt drauf zugegriffen
            _updatePackageMock = new UpdatePackageMock() { Access = _updatePackageAccessMock };

            _directoryMock = new DirectoryMock();

            _cleanupOldUnpackedFilesTest = new CleanupOldUnpackedFiles(_workFolder, _updatePackageMock, _directoryMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _cleanupOldUnpackedFilesTest = null;

            _commandResult = null;
            _updatePackageMock = null;
            _directoryMock = null;
        }

        [TestMethod]
        public void CleanupOldUnpackedFilesTest_Do_DeleteExistingDirectoryCalledAndReturnTrue()
        {
            var actual = _cleanupOldUnpackedFilesTest.Do(_commandResult);
            Assert.AreEqual(1, _directoryMock.DeleteCalled);

            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.PreviousErrors.Count());
            Assert.AreEqual(0, actual.ErrorsInThisCommand.Count());
        }
    }

}
