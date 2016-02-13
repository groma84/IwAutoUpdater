using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;

namespace IwAutoUpdater.DAL.Notifications
{
    public class NotificationReceiverFactory : INotificationReceiverFactory
    {
        INotificationReceiver INotificationReceiverFactory.CreateMailReceiver(string receiverMailAddress, string senderAddress, ISendMail sendMail, AddressUsernamePassword mailSettings)
        {
            return new MailReceiver(receiverMailAddress, senderAddress, mailSettings, sendMail);
        }
    }
}
