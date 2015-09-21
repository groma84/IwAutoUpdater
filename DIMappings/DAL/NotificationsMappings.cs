using IwAutoUpdater.DAL.Notifications;
using IwAutoUpdater.DAL.Notifications.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.DAL
{
    public class NotificationsMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<INotificationReceiverFactory, NotificationReceiverFactory>();
        }
    }
}
