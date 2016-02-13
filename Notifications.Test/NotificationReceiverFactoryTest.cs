using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace IwAutoUpdater.DAL.Notifications.Test
{
    [TestClass]
    public class NotificationReceiverFactoryTest
    {
        private INotificationReceiverFactory _notificationReceiverFactory;

        SendMailMock _sendMailMock;

        string _receiver = "receiver@testi.de";
        string _sender = "sender@testi.de";
        AddressUsernamePassword _eMailSettings = new AddressUsernamePassword();

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _sendMailMock = new SendMailMock();

            _notificationReceiverFactory = new NotificationReceiverFactory();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _notificationReceiverFactory = null;

            _sendMailMock = null;
        }

        [TestMethod]
        public void NotificationReceiverFactoryTest_CreateMailReceiver()
        {
            var actual = _notificationReceiverFactory.CreateMailReceiver(_receiver, _sender, _sendMailMock, _eMailSettings);
            Assert.IsNotNull(actual);
        }
    }
}
