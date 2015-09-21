using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.CommandPlanner.Contracts
{
    public interface IConfigurationConverter
    {
        IEnumerable<IUpdatePackage> ConvertServers(IEnumerable<ServerSettings> servers);
        IEnumerable<INotificationReceiver> ConvertMessageReceivers(IEnumerable<MessageReceiver> receivers, EMailSettings mailSettings);
    }
}
