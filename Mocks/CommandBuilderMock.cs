using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;

namespace Mocks
{
    public class CommandBuilderMock : ICommandBuilder
    {
        public List<Command> GetCommands = new List<Command>();
        public int GetCommandsCalled = 0;
        IEnumerable<Command> ICommandBuilder.GetCommands(string workFolder, IEnumerable<IUpdatePackage> updatePackages, IEnumerable<INotificationReceiver> notificationReceivers)
        {
            ++GetCommandsCalled;
            return GetCommands;
        }
    }
}
