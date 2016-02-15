using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;

namespace Mocks
{
    public class ConfigurationConverterMock : IConfigurationConverter
    {
        IEnumerable<INotificationReceiver> IConfigurationConverter.ConvertMessageReceivers(IEnumerable<MessageReceiver> receivers, AddressUsernamePassword mailSettings, string pickupDirectory, string fromEMailAddress)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IUpdatePackage> IConfigurationConverter.ConvertServers(IEnumerable<ServerSettings> servers)
        {
            throw new NotImplementedException();
        }
    }
}
