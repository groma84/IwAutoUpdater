using System;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using System.Linq;

namespace IwAutoUpdater.DAL.Updates
{
    internal class HttpDownloadAccess : IUpdatePackageAccess
    {
        private readonly ProxySettings _proxySettings;
        private readonly IHtmlGetter _htmlGetter;
        private string _url;

        public HttpDownloadAccess(string url, IHtmlGetter htmlGetter, ProxySettings proxySettings = null)
        {
            _url = url;
            _htmlGetter = htmlGetter;
            _proxySettings = proxySettings;
        }

        void IDisposable.Dispose()
        {
        }

        byte[] IUpdatePackageAccess.GetFile()
        {
            var result = _htmlGetter.DownloadFile(_url, _proxySettings);

            return result.FileContent;
        }

        string IUpdatePackageAccess.GetFilenameOnly()
        {
            return _url.Split('/').Last();
        }

        bool IUpdatePackageAccess.IsRemoteFileNewer(DateTime existingFileDate)
        {
            var serverLastModified = _htmlGetter.GetLastModifiedViaHead(_url, _proxySettings).Result;

            if (!serverLastModified.HasValue)
            {
                return false;
            }
            else
            {
                // zumindest der IIS schickt als Last Modified immer UTC, wir erwarten hier aber Maschinen-Lokalzeit
                var machineTimezone = TimeZone.CurrentTimeZone;
                return existingFileDate < machineTimezone.ToLocalTime(serverLastModified.Value);
            }
        }
    }
}