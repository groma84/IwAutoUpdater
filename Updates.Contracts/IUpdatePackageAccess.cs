using IwAutoUpdater.CrossCutting.Base;
using System;

namespace IwAutoUpdater.DAL.Updates.Contracts
{
    [DIIgnore]
    public interface IUpdatePackageAccess : IDisposable
    {
        bool IsRemoteFileNewer(DateTime existingFileDate);

        /// <summary>
        /// Ermittelt aus dem gesamten Pfad nur den Dateinamen inklusive Dateiendung
        /// </summary>
        /// <returns></returns>
        string GetFilenameOnly();

        /// <summary>
        /// Lädt die Datei runter
        /// </summary>
        /// <returns>true, wenn der Download erfolgreich ist</returns>
        byte[] GetFile();
    }
}