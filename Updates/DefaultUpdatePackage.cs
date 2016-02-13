using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.DAL.Updates
{
    public class DefaultUpdatePackage : IUpdatePackage
    {
        private readonly IUpdatePackageAccess _access;
        private readonly ServerSettings _settings;
        private readonly string _packageName;

        IUpdatePackageAccess IUpdatePackage.Access
        {
            get
            {
                return _access;
            }
        }

        string IUpdatePackage.PackageName
        {
            get
            {
                return _packageName;
            }
        }

        ServerSettings IUpdatePackage.Settings
        {
            get
            {
                return _settings;
            }
        }

        public DefaultUpdatePackage(string packageName, ServerSettings settings, IUpdatePackageAccess access)
        {
            _packageName = packageName;
            _settings = settings;
            _access = access;
        }
    }
}
