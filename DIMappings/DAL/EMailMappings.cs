using IwAutoUpdater.DAL.EMails;
using IwAutoUpdater.DAL.EMails.Contracts;
using IwAutoUpdater.DAL.ExternalCommands;
using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using IwAutoUpdater.DAL.WebAccess;
using IwAutoUpdater.DAL.WebAccess.Contracts;

namespace IwAutoUpdater.DIMappings.DAL
{
    public class EMailMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<ISendMail, SendMail>();
        }
    }
}
