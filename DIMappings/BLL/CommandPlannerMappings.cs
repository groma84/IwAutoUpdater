using IwAutoUpdater.BLL.CommandPlanner;
using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.BLL
{
  
   public class CommandPlannerMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<ICheckTimer, CheckTimer>();
            container.RegisterSingle<IConfigurationConverter, ConfigurationConverter>();
            container.RegisterSingle<ICommandBuilder, CommandBuilder>();
        }
    }
}
