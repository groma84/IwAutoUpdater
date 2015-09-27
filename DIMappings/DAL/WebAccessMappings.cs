using IwAutoUpdater.DAL.ExternalCommands;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.WebAccess;
using IwAutoUpdater.DAL.WebAccess.Contracts;

namespace IwAutoUpdater.DIMappings.DAL
{
    public class WebAccessMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IHtmlGetter, HtmlGetter>();
        }
    }
}
