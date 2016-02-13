using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;

namespace IwAutoUpdater.DAL.Notifications.Contracts
{
    public interface INotificationReceiverFactory
    {
        INotificationReceiver CreateMailReceiver(string receiverMailAddress, string senderAddress, ISendMail sendMail, AddressUsernamePassword mailSettings);
    }
}
