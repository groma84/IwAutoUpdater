using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.IO;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CheckIfNewerTest
    {
        static string _workFolder = @"C:\zork\";
        static string _fileName = "zork.zip";
        string _fullPath = Path.Combine(_workFolder, _fileName);
        UpdatePackageAccessMock _updatePackageAccessMock;
        UpdatePackageMock _updatePackageMock;
        SingleFileMock _singleFileMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock();

            _updatePackageMock = new UpdatePackageMock() { Access = _updatePackageAccessMock };

            _singleFileMock = new SingleFileMock();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _updatePackageAccessMock = null;
            _singleFileMock = null;
            _updatePackageMock = null;
        }

        [TestMethod]
        public void CheckIfNewerTest_LocalFileDoesNotExist_True()
        {
            _updatePackageAccessMock.IsRemoteFileNewer = true;
            _updatePackageAccessMock.GetFilenameOnly = _fileName;

            _singleFileMock.DoesExist = false;

            var cin = new CheckIfNewer(_workFolder, _updatePackageMock, _singleFileMock);
            var actual = cin.Do();
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            Assert.AreEqual(0, _singleFileMock.GetLastModifiedCalls);
            Assert.AreEqual(1, _updatePackageAccessMock.IsRemoteFileNewerCalls);
        }

        [TestMethod]
        public void CheckIfNewerTest_LocalFileNewer_False()
        {
            _updatePackageAccessMock.IsRemoteFileNewer = false;
            _updatePackageAccessMock.GetFilenameOnly = _fileName;

            _singleFileMock.DoesExist = true;
            _singleFileMock.GetLastModified.Add(_fullPath, new DateTime(2015, 5, 15));

            var cin = new CheckIfNewer(_workFolder, _updatePackageMock, _singleFileMock);
            var actual = cin.Do();
            Assert.IsFalse(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            Assert.AreEqual(1, _singleFileMock.GetLastModifiedCalls);
            Assert.AreEqual(1, _updatePackageAccessMock.IsRemoteFileNewerCalls);
        }

        [TestMethod]
        public void CheckIfNewerTest_LocalFileOlder_True()
        {
            _updatePackageAccessMock.IsRemoteFileNewer = true;
            _updatePackageAccessMock.GetFilenameOnly = _fileName;

            _singleFileMock.DoesExist = true;
            _singleFileMock.GetLastModified.Add(_fullPath, new DateTime(1995, 5, 15));

            var cin = new CheckIfNewer(_workFolder, _updatePackageMock, _singleFileMock);
            var actual = cin.Do();
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.Errors.Count());

            Assert.AreEqual(1, _singleFileMock.DoesExistCalls);
            Assert.AreEqual(1, _singleFileMock.GetLastModifiedCalls);
            Assert.AreEqual(1, _updatePackageAccessMock.IsRemoteFileNewerCalls);
        }
    }
}
