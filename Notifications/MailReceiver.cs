using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using System;

namespace IwAutoUpdater.DAL.Notifications
{
    public class MailReceiver : INotificationReceiver
    {
        private readonly string _from;
        private readonly ISendMail _sendMail;
        private readonly AddressUsernamePassword _mailSettings;
        private readonly string _to;

        public MailReceiver(string to, string from, AddressUsernamePassword mailSettings, ISendMail sendMail)
        {
            _to = to;
            _from = from;
            _sendMail = sendMail;
            _mailSettings = mailSettings;
        }

        bool INotificationReceiver.SendNotification(string subject, string message)
        {
            var mailData = new MailData()
            {
                From = _from,
                Recipients = new[] { _to },
                MessageBody = message,
                Subject = subject
            };

            try
            {
                _sendMail.Send(mailData, _mailSettings);
            }
            catch (Exception ex)
            {
                throw new NotificationSentException("E-Mail-Notification konnte nicht gesendet werden.", ex);
            }

            return true;
        }
    }
}
