using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.CommandPlanner.Contracts
{
    public interface ICheckTimer
    {
        bool IsCheckForUpdatesNecessary(int checkIntervalMinutes);
    }
}
