using IwAutoUpdater.CrossCutting.Logging;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.CrossCutting
{
    public class LoggingMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<ILogger, Logger>();
        }
    }
}
