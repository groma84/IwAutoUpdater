using System;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using System.Collections.Generic;

namespace Mocks
{
    public class NotificationReceiverFactoryMock : INotificationReceiverFactory
    {
        public Dictionary<string, INotificationReceiver> CreateMailReceiver = new Dictionary<string, INotificationReceiver>();
        public int CreateMailReceiverCalled = 0;
        INotificationReceiver INotificationReceiverFactory.CreateMailReceiver(string receiverMailAddress, AddressUsernamePassword mailSettings)
        {
            ++CreateMailReceiverCalled;
            return CreateMailReceiver[receiverMailAddress];
        }
    }
}