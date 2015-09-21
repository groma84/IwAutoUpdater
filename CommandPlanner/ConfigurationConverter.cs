using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.CrossCutting.Base;

namespace IwAutoUpdater.BLL.CommandPlanner
{
    public class ConfigurationConverter : IConfigurationConverter
    {
        private readonly INotificationReceiverFactory _notificationReceiverFactory;
        private readonly IUpdatePackageFactory _updatePackageFactory;

        public ConfigurationConverter(IUpdatePackageFactory updatePackageAccessFactory, INotificationReceiverFactory notificationReceiverFactory)
        {
            _updatePackageFactory = updatePackageAccessFactory;
            _notificationReceiverFactory = notificationReceiverFactory;
        }

        IEnumerable<INotificationReceiver> IConfigurationConverter.ConvertMessageReceivers(IEnumerable<MessageReceiver> receivers, EMailSettings mailSettings)
        {
            return receivers.Select(receiver =>
            {
                switch (receiver.Type)
                {
                    case MessageReceiverType.EMail:
                        return _notificationReceiverFactory.CreateMailReceiver(receiver.Receiver, mailSettings);

                    default:
                        throw new NotImplementedException($"Der Empfänger-Typ {receiver.Type} ist bisher nicht implementiert!");
                }
            }).ToArray();
        }

        IEnumerable<IUpdatePackage> IConfigurationConverter.ConvertServers(IEnumerable<ServerSettings> servers)
        {
            return servers.Select(s => _updatePackageFactory.Create(s)).ToArray();
        }
    }
}
