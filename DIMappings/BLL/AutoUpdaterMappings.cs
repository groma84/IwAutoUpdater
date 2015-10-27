using IwAutoUpdater.BLL.AutoUpdater;
using IwAutoUpdater.BLL.AutoUpdater.Contracts;
using IwAutoUpdater.BLL.CommandPlanner;
using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.BLL
{
  
   public class AutoUpdaterMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IAutoUpdaterThreadFactory, AutoUpdaterThreadFactory>();
            container.RegisterSingle<IAutoUpdaterWorker, AutoUpdaterWorker>();
            container.RegisterSingle<IAutoUpdaterCommandCreator, AutoUpdaterCommandCreator>();
        }
    }
}
