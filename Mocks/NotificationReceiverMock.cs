using IwAutoUpdater.DAL.Notifications.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocks
{
    public class NotificationReceiverMock : INotificationReceiver
    {
        public bool SendNotification = false;
        public int SendNotificationCalled = 0;
        public Exception SendNotificationThrowThisException = null;
        bool INotificationReceiver.SendNotification(string heading, string message)
        {
            ++SendNotificationCalled;

            if (SendNotificationThrowThisException != null)
            {
                throw SendNotificationThrowThisException;
            }

            return SendNotification;
        }
    }
}
