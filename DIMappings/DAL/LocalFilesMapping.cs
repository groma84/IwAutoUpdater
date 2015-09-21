using IwAutoUpdater.DAL.LocalFiles;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DIMappings.DAL
{
    public class LocalFilesMapping : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<ISingleFile, SingleFile>();
            container.RegisterSingle<IDirectory, Directory>();
            container.RegisterSingle<IDatabaseScript, IwAutoUpdater.DAL.LocalFiles.DatabaseScript>();
        }
    }
}
