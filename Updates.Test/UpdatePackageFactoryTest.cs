using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace IwAutoUpdater.DAL.Updates.Test
{
    [TestClass]
    public class UpdatePackageFactoryTest
    {
        private IUpdatePackageFactory _updatePackageFactory;
        private UpdatePackageAccessFactoryMock _updatePackageFactoryAccessMock;
        private ServerSettings _serverSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _serverSettings = new ServerSettings();
            _serverSettings.Type = GetDataMethod.LocalFile;
            _serverSettings.Path = "UpdatePackageFactoryTest_CreateDefaultCommand";

            _updatePackageFactoryAccessMock = new UpdatePackageAccessFactoryMock();
            _updatePackageFactoryAccessMock.CreateLocalFileAccess.Add(_serverSettings.Path, new LocalFileAccess(_serverSettings.Path));

            _updatePackageFactory = new UpdatePackageFactory(_updatePackageFactoryAccessMock);
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
