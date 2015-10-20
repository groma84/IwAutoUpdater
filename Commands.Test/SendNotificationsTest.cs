using IwAutoUpdater.DAL.Notifications.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class SendNotificationsTest
    {
        private SendNotifications _sendNotifications;

        private UpdatePackageMock _updatePackageMock;
        private NotificationReceiverMock _notificationReceiverMock;

        string _topic = "Subject";
        string _body = "Body";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.PackageName = "CheckUrlHttpStatusIs200Test";

            _notificationReceiverMock = new NotificationReceiverMock();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _sendNotifications = null;

            _updatePackageMock = null;
            _notificationReceiverMock = null;
        }

        [TestMethod]
        public void SendNotificationsTest_NotificationGetsSent_ResultIsTrue()
        {
            _notificationReceiverMock.SendNotification = true;

            _sendNotifications = new SendNotifications(new[] { _notificationReceiverMock }, _topic, _body, _updatePackageMock);

            var actual = _sendNotifications.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count() == 0);
            Assert.AreEqual(1, _notificationReceiverMock.SendNotificationCalled);
            Assert.IsTrue(actual.Successful);
        }

        [TestMethod]
        public void SendNotificationsTest_NotificationSentFails_ResultIsFalse_ErrorIsReturned()
        {
            _notificationReceiverMock.SendNotification = false;
            var exceptionToThrow = new NotificationSentException("Fehler");
            _notificationReceiverMock.SendNotificationThrowThisException = exceptionToThrow;

            _sendNotifications = new SendNotifications(new[] { _notificationReceiverMock }, _topic, _body, _updatePackageMock);

            var actual = _sendNotifications.Do();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual(actual.Errors.First().Exception, exceptionToThrow);
            Assert.AreEqual(1, _notificationReceiverMock.SendNotificationCalled);
            Assert.IsFalse(actual.Successful);
        }
    }
}
