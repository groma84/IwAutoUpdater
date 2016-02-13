using IwAutoUpdater.DAL.WebAccess.Contracts;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.WebAccess
{
    public class HtmlGetter : IHtmlGetter
    {
        HtmlDownload IHtmlGetter.DownloadHtml(string url)
        {
            return DownloadString(url);
        }

        HtmlDownload IHtmlGetter.DownloadHtml(string url, ProxySettings proxySettings)
        {
            if (proxySettings == null)
            {
                return DownloadString(url);
            }

            Action<WebClient> proxySettingsSetter = ((webClient) =>
            {
                webClient.Proxy = new WebProxy(proxySettings.Address);
                if (!String.IsNullOrEmpty(proxySettings.Username) && !String.IsNullOrEmpty(proxySettings.Password))
                {
                    webClient.Proxy.Credentials = new NetworkCredential(proxySettings.Username, proxySettings.Password);
                }
            });

            return DownloadString(url, proxySettingsSetter);
        }

        HtmlDownload IHtmlGetter.DownloadFile(string url, ProxySettings proxySettings)
        {
            if (proxySettings == null)
            {
                return DownloadFile(url);
            }

            Action<WebClient> proxySettingsSetter = ((webClient) =>
            {
                webClient.Proxy = new WebProxy(proxySettings.Address);
                if (!String.IsNullOrEmpty(proxySettings.Username) && !String.IsNullOrEmpty(proxySettings.Password))
                {
                    webClient.Proxy.Credentials = new NetworkCredential(proxySettings.Username, proxySettings.Password);
                }
            });

            return DownloadFile(url, proxySettingsSetter);
        }

        async Task<DateTime?> IHtmlGetter.GetLastModifiedViaHead(string url, ProxySettings proxySettings)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, url);
            HttpResponseMessage response = await httpClient.SendAsync(request);

            return response.Content.Headers.LastModified?.DateTime;
        }

        private HtmlDownload DownloadFile(string url, Action<WebClient> proxySettingsSetter = null)
        {
            return DownloadCore(
                url,
                proxySettingsSetter,
                (webClient) => new HtmlDownload { FileContent = webClient.DownloadData(url) },
                () => new HtmlDownload { FileContent = null, HttpStatusCode = 503 }
                );
        }

        private HtmlDownload DownloadString(string url, Action<WebClient> proxySettingsSetter = null)
        {
            return DownloadCore(
                 url,
                 proxySettingsSetter,
                 (webClient) => new HtmlDownload { Content = webClient.DownloadString(url) },
                 () => new HtmlDownload { Content = $"WebException caught: Timeout", HttpStatusCode = 503 }
                 );
        }

        private HtmlDownload DownloadCore(string url, Action<WebClient> proxySettingsSetter, Func<WebClientWithTimeout, HtmlDownload> download, Func<HtmlDownload> onTimeout)
        {
            var result = new HtmlDownload();

            var timeout = new TimeSpan(0, 5, 0);

            var webClient = new WebClientWithTimeout(timeout);

            if (proxySettingsSetter != null)
            {
                proxySettingsSetter(webClient);
            }

            try
            {
                result = download(webClient);
                result.HttpStatusCode = 200; // ohne Exception -> 200 aka OK
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    var webResponse = (HttpWebResponse)ex.Response;

                    result.HttpStatusCode = (int)(webResponse.StatusCode);
                    using (var sr = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result.Content = sr.ReadToEnd();
                    }

                    webResponse.Dispose();
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    return onTimeout();
                }
                else
                {
                    throw;
                }
            }

            return result;
        }
    }
}
