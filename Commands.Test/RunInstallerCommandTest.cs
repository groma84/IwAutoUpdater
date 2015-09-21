using IwAutoUpdater.CrossCutting.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class RunInstallerCommandTest
    {
        private RunInstallerCommand _runInstallerCommand;
        string _workFolder = @".";
        string _installerCommand = "RunInstallerCommandTestBatchFile.bat";
        string _installerCommandArguments = "testtest";

        CommandResult _commandResult;

        UpdatePackageMock _updatePackageMock;
        UpdatePackageAccessMock _updatePackageAccessMock;
        LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {

            TestCleanup();

            _commandResult = new CommandResult();

            _updatePackageAccessMock = new UpdatePackageAccessMock();
            _updatePackageAccessMock.GetFilenameOnly = "RunInstallerCommandTest.zip";

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.PackageName = "RunInstallerCommandTest";
            _updatePackageMock.Access = _updatePackageAccessMock;

            _loggerMock = new LoggerMock();

            TryDeleteFile();

            Directory.CreateDirectory(_updatePackageMock.PackageName);
            File.Copy(_installerCommand, Path.Combine(_updatePackageMock.PackageName, _installerCommand));

            _runInstallerCommand = new RunInstallerCommand(_installerCommand, _installerCommandArguments, _workFolder, _updatePackageMock, _loggerMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _runInstallerCommand = null;

            _commandResult = null;
            _updatePackageAccessMock = null;
            _loggerMock = null;

            TryDeleteFile();
        }

        private void TryDeleteFile()
        {
            if (_updatePackageAccessMock != null && Directory.Exists(_updatePackageMock.PackageName))
            {
                Directory.Delete(_updatePackageMock.PackageName, true);
            }
        }

        [TestMethod]
        public void RunInstallerCommandTest_RunExternalCommand()
        {
            var actual = _runInstallerCommand.Do(_commandResult);
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.ErrorsInThisCommand.Count());
            Assert.AreEqual(0, actual.PreviousErrors.Count());

            Assert.AreEqual(1, _updatePackageAccessMock.GetFilenameOnlyCalls);
            Assert.AreEqual(1, _loggerMock.InfoMessageCalled);
            Assert.AreEqual("abc testtest\r\n", _loggerMock.InfoMessages.First());
        }
    }
}
