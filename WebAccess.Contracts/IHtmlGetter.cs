using System;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.WebAccess.Contracts
{
    public interface IHtmlGetter
    {
        HtmlDownload DownloadHtml(string url);

        HtmlDownload DownloadHtml(string url, ProxySettings proxySettings);
        HtmlDownload DownloadFile(string url, ProxySettings proxySettings);
        Task<DateTime?> GetLastModifiedViaHead(string url, ProxySettings proxySettings);
    }
}
