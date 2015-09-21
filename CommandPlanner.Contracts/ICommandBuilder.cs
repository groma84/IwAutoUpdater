using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.CommandPlanner.Contracts
{
    public interface ICommandBuilder
    {
        IEnumerable<Command> GetCommands(String workFolder, IEnumerable<IUpdatePackage> updatePackages, IEnumerable<INotificationReceiver> notificationReceivers);
    }
}
