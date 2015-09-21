using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.CommandPlanner.Test
{
    [TestClass]
    public class ConfigurationConverterTest
    {
        private IConfigurationConverter _configurationConverter;
        private IUpdatePackageFactory _updatePackageFactoryMock;
        private NotificationReceiverFactoryMock _notificationReceiverFactoryMock;

        string _path = @"C:\zonk\bonk.zip";
        UpdatePackageAccessMock _mockPackageAccess = new UpdatePackageAccessMock();
        EMailSettings _mailSettings = new EMailSettings();
        string _mailAddress = @"Testi@testi.de";
        NotificationReceiverMock _mockMailReceiver = new NotificationReceiverMock();

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageFactoryMock = new UpdatePackageFactoryMock();            

            _notificationReceiverFactoryMock = new NotificationReceiverFactoryMock();

            _configurationConverter = new ConfigurationConverter(_updatePackageFactoryMock, _notificationReceiverFactoryMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _configurationConverter = null;

            _updatePackageFactoryMock = null;
            _notificationReceiverFactoryMock = null;
        }

        [TestMethod]
        public void ConfigurationConverterTest_ConvertServers_Empty()
        {
            var actual = _configurationConverter.ConvertServers(new ServerSettings[0]);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void ConfigurationConverterTest_ConvertServers_LocalFile()
        {           
            var localFile = new ServerSettings[]
            {
                new ServerSettings()
                {
                    DownloadOnly = false,
                    Path = _path,
                    SkipDatabaseUpdate = false,
                    Type = GetDataMethod.LocalFile
                }
            };

            var actual = _configurationConverter.ConvertServers(localFile).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(1, (_updatePackageFactoryMock as UpdatePackageFactoryMock).CreateCalled);
        }

        [TestMethod]
        public void ConfigurationConverterTest_ConvertMessageReceivers_Empty()
        {
            var actual = _configurationConverter.ConvertMessageReceivers(new MessageReceiver[0], _mailSettings);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void ConfigurationConverterTest_ConvertMessageReceivers_EMail()
        {
            _notificationReceiverFactoryMock.CreateMailReceiver.Add(_mailAddress, _mockMailReceiver);

            var mailReceiver = new MessageReceiver[]
            {
                new MessageReceiver()
                {
                   Receiver = _mailAddress,
                   Type = MessageReceiverType.EMail
                }
            };

            var actual = _configurationConverter.ConvertMessageReceivers(mailReceiver, _mailSettings).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(_mockMailReceiver, actual[0]);
            Assert.AreEqual(1, _notificationReceiverFactoryMock.CreateMailReceiverCalled);
        }
    }
}
