using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System.Collections.Generic;
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
