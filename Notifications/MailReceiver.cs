using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Notifications
{
    public class MailReceiver : INotificationReceiver
    {
        private readonly EMailSettings _mailSettings;
        private readonly string _address;

        public MailReceiver(string address, EMailSettings mailSettings)
        {
            _address = address;
            _mailSettings = mailSettings;
        }

        bool INotificationReceiver.SendNotification(string heading, string message)
        {
            throw new NotImplementedException();
        }
    }
}
