using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Notifications.Contracts
{
    public interface INotificationReceiverFactory
    {
        INotificationReceiver CreateMailReceiver(string receiverMailAddress, AddressUsernamePassword mailSettings);
    }
}
