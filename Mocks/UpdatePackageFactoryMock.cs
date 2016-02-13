using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;

namespace Mocks
{
    public class UpdatePackageFactoryMock : IUpdatePackageFactory
    {
        public IUpdatePackage Create = null;
        public int CreateCalled = 0;
        IUpdatePackage IUpdatePackageFactory.Create(ServerSettings serverSettings)
        {
            ++CreateCalled;
            return Create;
        }
    }
}