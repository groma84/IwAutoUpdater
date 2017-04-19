using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;

namespace IwAutoUpdater.DAL.Updates.Contracts
{
    public interface IUpdatePackageAccessFactory
    {
        IUpdatePackageAccess CreateLocalFileAccess(string filePath, ILogger logger);

        /// <summary>
        /// Der Zugriff auf die Dateifreigabe erfolgt immer mit dem Benutzer, der das Programm ausführt
        /// </summary>
        /// <param name="uncPath"></param>
        /// <returns></returns>
        IUpdatePackageAccess CreateUncPathAccess(string uncPath, ILogger logger);

        /// <summary>
        ///  
        /// </summary>
        /// <param name="url"></param>
        /// <param name="htmlGetter"></param>
        /// <param name="proxySettings">Kann auch null sein, wenn kein Proxy genutzt werden soll</param>
        /// <returns></returns>
        IUpdatePackageAccess CreateHttpDownloadAccess(string url, string username, string password, IHtmlGetter htmlGetter, ILogger logger, ProxySettings proxySettings);
    }
}
