using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.Collections.Generic;

namespace IwAutoUpdater.BLL.CommandPlanner.Contracts
{
    public interface IConfigurationConverter
    {
        IEnumerable<IUpdatePackage> ConvertServers(IEnumerable<ServerSettings> servers);
        IEnumerable<INotificationReceiver> ConvertMessageReceivers(IEnumerable<MessageReceiver> receivers, AddressUsernamePassword mailSettings, string pickupDirectory, string fromEMailAddress);
    }
}
