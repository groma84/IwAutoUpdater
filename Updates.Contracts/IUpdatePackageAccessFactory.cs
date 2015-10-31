namespace IwAutoUpdater.DAL.Updates.Contracts
{
    public interface IUpdatePackageAccessFactory
    {
        IUpdatePackageAccess CreateLocalFileAccess(string filePath);

        /// <summary>
        /// Der Zugriff auf die Dateifreigabe erfolgt immer mit dem Benutzer, der das Programm ausführt
        /// </summary>
        /// <param name="uncPath"></param>
        /// <returns></returns>
        IUpdatePackageAccess CreateUncPathAccess(string uncPath);


        IUpdatePackageAccess CreateHttpDownloadAccess(string url);
    }
}
