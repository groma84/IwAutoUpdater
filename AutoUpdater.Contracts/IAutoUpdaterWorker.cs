using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater.Contracts
{
    public interface IAutoUpdaterWorker
    {
        Task NeverendingWorkLoop();
    }
}
