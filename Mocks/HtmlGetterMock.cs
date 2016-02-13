using IwAutoUpdater.DAL.WebAccess.Contracts;

namespace Mocks
{
    public class HtmlGetterMock : IHtmlGetter
    {

        public HtmlDownload DownloadHtml = null;
        public int DownloadHtmlCalled = 0;
        HtmlDownload IHtmlGetter.DownloadHtml(string url)
        {
            ++DownloadHtmlCalled;
            return DownloadHtml;
        }

        public HtmlDownload DownloadHtmlWithProxy = null;
        public int DownloadHtmlCalledWithProxy = 0;
        HtmlDownload IHtmlGetter.DownloadHtml(string url, ProxySettings proxySettings)
        {
            ++DownloadHtmlCalledWithProxy;
            return DownloadHtmlWithProxy;
        }
    }
}