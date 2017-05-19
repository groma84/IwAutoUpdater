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
        HtmlDownload IHtmlGetter.DownloadHtml(string url, string username, string password)
        {
            return DownloadString(url, username, password);
        }

        HtmlDownload IHtmlGetter.DownloadHtml(string url, string username, string password, ProxySettings proxySettings)
        {
            if (proxySettings == null)
            {
                return DownloadString(url, username, password);
            }

            return DownloadString(url, username, password, ProxySettingsSetterCreator(proxySettings));
        }

        HtmlDownload IHtmlGetter.DownloadFile(string url, string username, string password, ProxySettings proxySettings)
        {
            if (proxySettings == null)
            {
                return DownloadFile(url, username, password);
            }

            return DownloadFile(url, username, password, ProxySettingsSetterCreator(proxySettings));
        }

        async Task<DateTime?> IHtmlGetter.GetLastModifiedViaHead(string url, string username, string password, ProxySettings proxySettings)
        {
            HttpResponseMessage response = null;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, url);

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(username, password), PreAuthenticate = true })
                {
                    using (var client = new HttpClient(handler))
                    {
                        response = await client.SendAsync(request);
                    }
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    response = await client.SendAsync(request);
                }
            }

            return response.Content.Headers.LastModified?.DateTime;
        }

        private HtmlDownload DownloadFile(string url, string username, string password, Action<WebClient> proxySettingsSetter = null)
        {
            return DownloadCore(
                url,
                username,
                password,
                proxySettingsSetter,
                (webClient) => new HtmlDownload { FileContent = webClient.DownloadData(url) },
                () => new HtmlDownload { FileContent = null, HttpStatusCode = 503 }
                );
        }

        private HtmlDownload DownloadString(string url, string username, string password, Action<WebClient> proxySettingsSetter = null)
        {
            return DownloadCore(
                 url,
                 username,
                 password,
                 proxySettingsSetter,
                 (webClient) => new HtmlDownload { Content = webClient.DownloadString(url) },
                 () => new HtmlDownload { Content = $"WebException caught: Timeout", HttpStatusCode = 503 }
                 );
        }

        private HtmlDownload DownloadCore(string url, string username, string password, Action<WebClient> proxySettingsSetter, Func<WebClientWithTimeout, HtmlDownload> download, Func<HtmlDownload> onTimeout)
        {
            var result = new HtmlDownload();

            var timeout = new TimeSpan(0, 5, 0);

            var webClient = new WebClientWithTimeout(timeout);

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                webClient.Credentials = new NetworkCredential(username, password);
            }

            proxySettingsSetter?.Invoke(webClient);

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

        private static Action<WebClient> ProxySettingsSetterCreator(ProxySettings proxySettings)
        {
            return new Action<WebClient>(webClient =>
            {
                webClient.Proxy = new WebProxy(proxySettings.Address);
                if (!String.IsNullOrEmpty(proxySettings.Username) && !String.IsNullOrEmpty(proxySettings.Password))
                {
                    webClient.Proxy.Credentials = new NetworkCredential(proxySettings.Username, proxySettings.Password);
                }
            });
        }
    }
}
