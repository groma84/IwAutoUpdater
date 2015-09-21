using IwAutoUpdater.CrossCutting.Configuration;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.DIMappings.CrossCutting
{
    public class ConfigurationMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IConfigurationFileAccess, ConfigurationFileAccess>();
            container.RegisterSingle<IConfiguration, JsonFileConfiguration>();
        }
    }
}
