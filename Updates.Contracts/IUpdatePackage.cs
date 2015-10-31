using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.DAL.Updates.Contracts
{
    [DIIgnore]
    public interface IUpdatePackage
    {
        IUpdatePackageAccess Access { get; }

        string PackageName { get; }

        ServerSettings Settings { get; }
    }
}
