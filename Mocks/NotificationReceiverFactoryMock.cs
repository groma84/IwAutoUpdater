using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;
using System;

namespace Mocks
{
    public class NotificationReceiverFactoryMock : INotificationReceiverFactory
    {
        public INotificationReceiver CreateMailReceiver = null;
        public int CreateMailReceiverCalled = 0;

        INotificationReceiver INotificationReceiverFactory.CreateMailReceiver(string receiverMailAddress, string senderAddress, ISendMail sendMail, string pickupDirectory)
        {
            throw new NotImplementedException();
        }

        INotificationReceiver INotificationReceiverFactory.CreateMailReceiver(string receiverMailAddress, string senderAddress, ISendMail sendMail, AddressUsernamePassword mailSettings)
        {
            ++CreateMailReceiverCalled;
            return CreateMailReceiver;
        }
    }
}