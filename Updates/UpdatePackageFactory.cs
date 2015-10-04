using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Updates
{
    public class UpdatePackageFactory : IUpdatePackageFactory
    {
        private readonly IUpdatePackageAccessFactory _updatePackageAccessFactory;

        public UpdatePackageFactory(IUpdatePackageAccessFactory updatePackageAccessFactory)
        {
            _updatePackageAccessFactory = updatePackageAccessFactory;
        }

        IUpdatePackage IUpdatePackageFactory.Create(ServerSettings serverSettings)
        {
            IUpdatePackageAccess updatePackageAccess = null;

            switch (serverSettings.Type)
            {
                case GetDataMethod.LocalFile:
                    updatePackageAccess = _updatePackageAccessFactory.CreateLocalFileAccess(serverSettings.Path);
                    break;

                case GetDataMethod.UncPath:
                    updatePackageAccess = _updatePackageAccessFactory.CreateUncPathAccess(serverSettings.Path, serverSettings.GetDataUsername, serverSettings.GetDataPassword);
                    break;

                case GetDataMethod.HttpDownload:
                    updatePackageAccess = _updatePackageAccessFactory.CreateHttpDownloadAccess(serverSettings.Path);
                    break;

                default:
                    throw new NotImplementedException($"Der serverSettings-Typ {serverSettings.Type} ist bisher nicht implementiert!");
            }

            return new DefaultUpdatePackage(serverSettings.Path, serverSettings, updatePackageAccess);
        }
    }
}
