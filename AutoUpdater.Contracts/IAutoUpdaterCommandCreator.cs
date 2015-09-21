using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater.Contracts
{
    public interface IAutoUpdaterCommandCreator
    {
        Task NeverendingCreationLoop(Settings settings);
    }
}
