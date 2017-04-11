using IwAutoUpdater.CrossCutting.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.IO;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class UnzipFileTest
    {
        static string _workFolder = @"./UnzipFileTest/";
        static string _fileName = "UnzipFileTest.zip";
        static string _fileNameWithPw = "UnzipFileTestPw.zip";

        string _contentFileName = @"Wetterstationen.xml";

        UpdatePackageMock _updatePackageMock;
        private UpdatePackageAccessMock _updatePackageAccessMock;
        CommandResult _commandResult = new CommandResult();

        private UnzipFile _unzipFile;
        private DirectoryMock _directoryMock;
        private LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _directoryMock = new DirectoryMock();
            _loggerMock = new LoggerMock();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(_workFolder))
            {
                foreach (var f in Directory.EnumerateFiles(_workFolder, _contentFileName))
                {
                    File.Delete(f);
                }
            }

            _unzipFile = null;
            _updatePackageMock = null;
            _updatePackageAccessMock = null;
        }

        [TestMethod]
        public void UnzipFileTest_Do()
        {
            _updatePackageAccessMock = new UpdatePackageAccessMock() { GetFilenameOnly = _fileName };

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.Access = _updatePackageAccessMock;

            _unzipFile = new UnzipFile(_workFolder, null, _updatePackageMock, _directoryMock, _loggerMock);

            var actual = _unzipFile.Do();
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _updatePackageAccessMock.GetFilenameOnlyCalls);
            Assert.IsTrue(Directory.Exists(_workFolder));
            Assert.IsTrue(File.Exists(Path.Combine(_workFolder, _contentFileName)));
        }

        [TestMethod]
        public void UnzipFileTest_Do_WithPassword()
        {
            _updatePackageAccessMock = new UpdatePackageAccessMock() { GetFilenameOnly = _fileNameWithPw };

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.Access = _updatePackageAccessMock;

            _unzipFile = new UnzipFile(_workFolder, "passwort", _updatePackageMock, _directoryMock, _loggerMock);
            var actual = _unzipFile.Do();
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _updatePackageAccessMock.GetFilenameOnlyCalls);
            Assert.IsTrue(Directory.Exists(_workFolder));
            Assert.IsTrue(File.Exists(Path.Combine(_workFolder, _contentFileName)));
        }
    }
}
