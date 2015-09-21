using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Notifications.Test
{
    [TestClass]
    public class NotificationReceiverFactoryTest
    {
        private INotificationReceiverFactory _notificationReceiverFactory;

        string _receiver = "testi@testi.de";
        EMailSettings _eMailSettings = new EMailSettings();

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _notificationReceiverFactory = new NotificationReceiverFactory();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _notificationReceiverFactory = null;
        }

        [TestMethod]
        public void NotificationReceiverFactoryTest_CreateMailReceiver()
        {
            var actual = _notificationReceiverFactory.CreateMailReceiver(_receiver, _eMailSettings);
            Assert.IsNotNull(actual);
        }
    }
}
