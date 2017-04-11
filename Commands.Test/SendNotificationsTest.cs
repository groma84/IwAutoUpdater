using IwAutoUpdater.DAL.Notifications.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using Moq;
using SFW.Contracts;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class SendNotificationsTest
    {
        private SendNotifications _sendNotifications;

        private UpdatePackageMock _updatePackageMock;
        private NotificationReceiverMock _notificationReceiverMock;

        private NowGetterMock _nowGetterMock;
        private Mock<IBlackboard> _blackboardMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.PackageName = "CheckUrlHttpStatusIs200Test";

            _notificationReceiverMock = new NotificationReceiverMock();

            _nowGetterMock = new NowGetterMock();
            _nowGetterMock.Now = new System.DateTime(2015, 5, 15, 11, 0, 0);

            _blackboardMock = new Mock<IBlackboard>();
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

            _sendNotifications = new SendNotifications(new[] { _notificationReceiverMock }, false, _updatePackageMock, _nowGetterMock, _blackboardMock.Object);

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

            _sendNotifications = new SendNotifications(new[] { _notificationReceiverMock }, false, _updatePackageMock, _nowGetterMock, _blackboardMock.Object);

            var actual = _sendNotifications.Do();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual(actual.Errors.First().Exception, exceptionToThrow);
            Assert.AreEqual(1, _notificationReceiverMock.SendNotificationCalled);
            Assert.IsFalse(actual.Successful);
        }
    }
}
