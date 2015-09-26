using IwAutoUpdater.DAL.ExternalCommands;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;

namespace IwAutoUpdater.DIMappings.DAL
{
    public class ExternalCommandsMapping : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IRunExternalCommand, ExternalCommandRunner>();
        }
    }
}
