using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.DAL.Updates.Contracts
{
    public interface IUpdatePackageFactory
    {
        IUpdatePackage Create(ServerSettings serverSettings);
    }
}
