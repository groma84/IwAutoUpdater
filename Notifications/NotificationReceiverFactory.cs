using IwAutoUpdater.DAL.Notifications.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.DAL.Notifications
{
    public class NotificationReceiverFactory : INotificationReceiverFactory
    {
        INotificationReceiver INotificationReceiverFactory.CreateMailReceiver(string receiverMailAddress, EMailSettings mailSettings)
        {
            return new MailReceiver(receiverMailAddress, mailSettings);

        }
    }
}
