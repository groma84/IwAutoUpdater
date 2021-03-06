﻿using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using Moq;

namespace IwAutoUpdater.DAL.Updates.Test
{
    [TestClass]
    public class UpdatePackageFactoryTest
    {
        private IUpdatePackageFactory _updatePackageFactory;
        private UpdatePackageAccessFactoryMock _updatePackageFactoryAccessMock;
        private ServerSettings _serverSettings;
        private Mock<IHtmlGetter> _htmlGetterMock;
        private ILogger _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _loggerMock = new LoggerMock();

            _serverSettings = new ServerSettings();
            _serverSettings.Type = GetDataMethod.LocalFile;
            _serverSettings.Path = "UpdatePackageFactoryTest_CreateDefaultCommand";

            _updatePackageFactoryAccessMock = new UpdatePackageAccessFactoryMock();
            _updatePackageFactoryAccessMock.CreateLocalFileAccess.Add(_serverSettings.Path, new LocalFileAccess(_serverSettings.Path, _loggerMock));

            _htmlGetterMock = new Mock<IHtmlGetter>();
            
            _updatePackageFactory = new UpdatePackageFactory(_updatePackageFactoryAccessMock, _htmlGetterMock.Object, _loggerMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _updatePackageFactory = null;

            _serverSettings = null;
            _updatePackageFactoryAccessMock = null;
        }

        [TestMethod]
        public void UpdatePackageFactoryTest_CreateDefaultCommand()
        {
            var actual = _updatePackageFactory.Create(_serverSettings);
            Assert.IsNotNull(actual);
            Assert.AreEqual(typeof(LocalFileAccess), actual.Access.GetType());
            Assert.AreEqual(_serverSettings.Path, actual.PackageName);
            Assert.AreEqual(_serverSettings.Path, actual.Settings.Path);
        }
    }
}
