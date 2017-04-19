using System;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.WebAccess.Contracts
{
    public interface IHtmlGetter
    {
        HtmlDownload DownloadHtml(string url, string username, string password);

        HtmlDownload DownloadHtml(string url, string username, string password, ProxySettings proxySettings);
        HtmlDownload DownloadFile(string url, string username, string password, ProxySettings proxySettings);
        Task<DateTime?> GetLastModifiedViaHead(string url, ProxySettings proxySettings);
    }
}
