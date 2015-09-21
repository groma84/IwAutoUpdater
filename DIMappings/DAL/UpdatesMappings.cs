using IwAutoUpdater.DAL.Updates;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.DAL
{
    public class UpdatesMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IUpdatePackageAccessFactory, UpdatePackageAccessFactory>();
            container.RegisterSingle<IUpdatePackageFactory, UpdatePackageFactory>();
        }
    }
}
