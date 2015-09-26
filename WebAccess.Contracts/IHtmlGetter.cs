namespace IwAutoUpdater.DAL.WebAccess.Contracts
{
    public interface IHtmlGetter
    {
        HtmlDownload DownloadHtml(string url);

        HtmlDownload DownloadHtml(string url, ProxySettings proxySettings);
    }
}
