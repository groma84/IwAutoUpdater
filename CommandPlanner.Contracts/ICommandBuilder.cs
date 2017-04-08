using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;

namespace IwAutoUpdater.BLL.CommandPlanner.Contracts
{
    public interface ICommandBuilder
    {
        IEnumerable<Command> GetCommands(string workFolder, IEnumerable<IUpdatePackage> updatePackages, IEnumerable<INotificationReceiver> notificationReceivers);
    }
}
