using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.Linq;

namespace IwAutoUpdater.BLL.CommandPlanner.Test
{
    [TestClass]
    public class ConfigurationConverterTest
    {
        private IConfigurationConverter _configurationConverter;
        private IUpdatePackageFactory _updatePackageFactoryMock;
        private NotificationReceiverFactoryMock _notificationReceiverFactoryMock;

        string _path = @"C:\zonk\bonk.zip";
        string _sender = "sender";
        UpdatePackageAccessMock _mockPackageAccess = new UpdatePackageAccessMock();
        AddressUsernamePassword _mailSettings = new AddressUsernamePassword();
        string _mailAddress = @"Testi@testi.de";
        NotificationReceiverMock _mockMailReceiver = new NotificationReceiverMock();
        SendMailMock _sendMailMock = new SendMailMock();

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageFactoryMock = new UpdatePackageFactoryMock();

            _notificationReceiverFactoryMock = new NotificationReceiverFactoryMock();

            _configurationConverter = new ConfigurationConverter(_updatePackageFactoryMock, _notificationReceiverFactoryMock, _sendMailMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _configurationConverter = null;

            _updatePackageFactoryMock = null;
            _notificationReceiverFactoryMock = null;
            _sendMailMock = null;
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
            var actual = _configurationConverter.ConvertMessageReceivers(new MessageReceiver[0], _mailSettings, null, _sender);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void ConfigurationConverterTest_ConvertMessageReceivers_EMail()
        {
            _notificationReceiverFactoryMock.CreateMailReceiver = _mockMailReceiver;

            var mailReceiver = new MessageReceiver[]
            {
                new MessageReceiver()
                {
                   Receiver = _mailAddress,
                   Type = MessageReceiverType.EMail
                }
            };

            var actual = _configurationConverter.ConvertMessageReceivers(mailReceiver, _mailSettings, null, _sender).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(_mockMailReceiver, actual[0]);
            Assert.AreEqual(1, _notificationReceiverFactoryMock.CreateMailReceiverCalled);
        }
    }
}
