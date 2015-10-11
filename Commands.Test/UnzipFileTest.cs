﻿using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
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
    public class UnzipFileTest
    {
        static string _workFolder = @"./UnzipFileTest/";
        static string _fileName = "UnzipFileTest.zip";
        UpdatePackageMock _updatePackageMock;
        private UpdatePackageAccessMock _updatePackageAccessMock;
        CommandResult _commandResult = new CommandResult();

        private UnzipFile _unzipFile;

        [TestInitialize]
        public void TestInitialize()
        {           
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock() { GetFilenameOnly = _fileName };

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.Access = _updatePackageAccessMock;

            _unzipFile = new UnzipFile(_workFolder, _updatePackageMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(_workFolder) && Directory.EnumerateFiles(_workFolder).Any(name => !name.EndsWith(".zip")))
            {
                Directory.Delete(_workFolder, true);
            }

            _unzipFile = null;
            _updatePackageMock = null;
            _updatePackageAccessMock = null;
        }

        [TestMethod]
        public void UnzipFileTest_Do()
        {
            var actual = _unzipFile.Do(_commandResult);
            Assert.IsTrue(actual.Successful);
            Assert.AreEqual(0, actual.ErrorsInThisCommand.Count());
            Assert.AreEqual(0, actual.PreviousErrors.Count());

            Assert.AreEqual(1, _updatePackageAccessMock.GetFilenameOnlyCalls);
            Assert.IsTrue(Directory.Exists(_workFolder));
            Assert.IsTrue(File.Exists(Path.Combine(_workFolder, "Wetterstationen.xml")));
        }
    }
}
