using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocks
{
    public class CheckTimerMock : ICheckTimer
    {
        public bool IsCheckForUpdatesNecessary = false;
        public int IsCheckForUpdatesNecessaryCalled = 0;
        bool ICheckTimer.IsCheckForUpdatesNecessary(int checkIntervalMinutes)
        {
            ++IsCheckForUpdatesNecessaryCalled;
            return IsCheckForUpdatesNecessary;
        }
    }
}
