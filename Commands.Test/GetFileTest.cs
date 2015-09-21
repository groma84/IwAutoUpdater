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
    public class GetFileTest
    {
        static string _workFolder = @"C:\zork\";
        static string _fileName = "zork.zip";
        string _fullPath = Path.Combine(_workFolder, _fileName);
        UpdatePackageAccessMock _updatePackageAccessMock;
        UpdatePackageMock _updatePackageMock;
        SingleFileMock _singleFileMock;
        CommandResult _commandResult = new CommandResult();

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock();

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.Access = _updatePackageAccessMock;

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
        public void GetFileTest_Successful()
        {
            _singleFileMock.Write.Add(_fullPath, true);
            _updatePackageAccessMock.GetFile = new byte[] { byte.Parse("123") };
            _updatePackageAccessMock.GetFilenameOnly = _fileName;

            var gf = new GetFile(_workFolder, _updatePackageMock, _singleFileMock);
            var actual = gf.Do(_commandResult);
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.ErrorsInThisCommand.Count());
            Assert.AreEqual(0, actual.PreviousErrors.Count());

            Assert.AreEqual(1, _singleFileMock.WriteCalls);
            Assert.AreEqual(1, _updatePackageAccessMock.GetFileCalls);
        }
    }
}
