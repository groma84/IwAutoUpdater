using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using System;

namespace IwAutoUpdater.DAL.Updates
{
    public class UpdatePackageFactory : IUpdatePackageFactory
    {
        private readonly ILogger _logger;
        private readonly IHtmlGetter _htmlGetter;
        private readonly IUpdatePackageAccessFactory _updatePackageAccessFactory;

        public UpdatePackageFactory(IUpdatePackageAccessFactory updatePackageAccessFactory, IHtmlGetter htmlGetter, ILogger logger)
        {
            _updatePackageAccessFactory = updatePackageAccessFactory;
            _htmlGetter = htmlGetter;
            _logger = logger;
        }

        IUpdatePackage IUpdatePackageFactory.Create(ServerSettings serverSettings)
        {
            IUpdatePackageAccess updatePackageAccess = null;

            switch (serverSettings.Type)
            {
                case GetDataMethod.LocalFile:
                    updatePackageAccess = _updatePackageAccessFactory.CreateLocalFileAccess(serverSettings.Path, _logger);
                    break;

                case GetDataMethod.UncPath:
                    updatePackageAccess = _updatePackageAccessFactory.CreateUncPathAccess(serverSettings.Path, _logger);
                    break;

                case GetDataMethod.HttpDownload:
                    ProxySettings proxySettings = CreateProxySettings(serverSettings);
                    updatePackageAccess = _updatePackageAccessFactory.CreateHttpDownloadAccess(serverSettings.Path, serverSettings.HttpDownloadUsername, serverSettings.HttpDownloadPassword, _htmlGetter, _logger, proxySettings);
                    break;

                default:
                    throw new NotImplementedException($"Der serverSettings-Typ {serverSettings.Type} ist bisher nicht implementiert!");
            }

            return new DefaultUpdatePackage(serverSettings.Path, serverSettings, updatePackageAccess);
        }

        private static ProxySettings CreateProxySettings(ServerSettings serverSettings)
        {
            ProxySettings proxySettings = null;
            if (serverSettings.CheckUrlProxySettings != null)
            {
                proxySettings = new ProxySettings();
                proxySettings.Address = serverSettings.CheckUrlProxySettings.Address;

                if (!String.IsNullOrEmpty(serverSettings.CheckUrlProxySettings.Password) && !String.IsNullOrEmpty(serverSettings.CheckUrlProxySettings.Username))
                {
                    proxySettings.Username = serverSettings.CheckUrlProxySettings.Username;
                    proxySettings.Password = serverSettings.CheckUrlProxySettings.Password;
                }
            }

            return proxySettings;
        }
    }
}
