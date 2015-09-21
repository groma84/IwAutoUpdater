using IwAutoUpdater.CrossCutting.SFW;
using IwAutoUpdater.CrossCutting.SFW.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.CrossCutting
{
    public class SFWMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<INowGetter, NowGetter>();
        }
    }
}
