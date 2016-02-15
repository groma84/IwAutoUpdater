using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.EMails.Contracts;

namespace IwAutoUpdater.BLL.CommandPlanner
{
    public class ConfigurationConverter : IConfigurationConverter
    {
        private readonly ISendMail _sendMail;
        private readonly INotificationReceiverFactory _notificationReceiverFactory;
        private readonly IUpdatePackageFactory _updatePackageFactory;

        public ConfigurationConverter(IUpdatePackageFactory updatePackageAccessFactory, INotificationReceiverFactory notificationReceiverFactory, ISendMail sendMail)
        {
            _updatePackageFactory = updatePackageAccessFactory;
            _notificationReceiverFactory = notificationReceiverFactory;
            _sendMail = sendMail;
        }

        IEnumerable<INotificationReceiver> IConfigurationConverter.ConvertMessageReceivers(IEnumerable<MessageReceiver> receivers, AddressUsernamePassword mailSettings, string pickupDirectory, string fromEMailAddress)
        {
            return receivers.Select(receiver =>
            {
                switch (receiver.Type)
                {
                    case MessageReceiverType.EMail:
                        if (string.IsNullOrEmpty(pickupDirectory))
                        {
                            return _notificationReceiverFactory.CreateMailReceiver(receiver.Receiver, fromEMailAddress, _sendMail, mailSettings);
                        }
                        else
                        {
                            return _notificationReceiverFactory.CreateMailReceiver(receiver.Receiver, fromEMailAddress, _sendMail, pickupDirectory);
                        }

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
