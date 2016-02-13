using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.IO;
using System.Linq;

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
        RunExternalCommandMock _runExternalCommandMock;

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

            _runExternalCommandMock = new RunExternalCommandMock();

            TryDeleteFile();

            Directory.CreateDirectory(_updatePackageMock.PackageName);
            File.Copy(_installerCommand, Path.Combine(_updatePackageMock.PackageName, _installerCommand));

            _runInstallerCommand = new RunInstallerCommand(_installerCommand, _installerCommandArguments, _workFolder, _updatePackageMock, _runExternalCommandMock, _loggerMock);
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
            _runExternalCommandMock.Run = new ExternalCommandResult()
            {
                ExitCode = 0,
                RecordedStandardOutput = "abc testtest\r\n"
            };

            var actual = _runInstallerCommand.Do();
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _updatePackageAccessMock.GetFilenameOnlyCalls);
            Assert.AreEqual(1, _loggerMock.InfoMessageCalled);
            Assert.AreEqual(1, _runExternalCommandMock.RunCalled);

            Assert.AreEqual("abc testtest\r\n", _loggerMock.InfoMessages.First());
        }
    }
}
