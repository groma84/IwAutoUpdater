using IwAutoUpdater.DAL.WebAccess.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.WebAccess
{
    public class HtmlGetter : IHtmlGetter
    {
        HtmlDownload IHtmlGetter.DownloadHtml(string url)
        {
            return Download(url);
        }

        HtmlDownload IHtmlGetter.DownloadHtml(string url, ProxySettings proxySettings)
        {
            if (proxySettings == null)
            {
                return Download(url);
            }

            Action<WebClient> proxySettingsSetter = ((webClient) =>
            {
                webClient.Proxy = new WebProxy(proxySettings.Address);
                if (!String.IsNullOrEmpty(proxySettings.Username) && !String.IsNullOrEmpty(proxySettings.Password))
                {
                    webClient.Proxy.Credentials = new NetworkCredential(proxySettings.Username, proxySettings.Password);
                }
            });
            
            return Download(url, proxySettingsSetter);
        }

        private HtmlDownload Download(string url, Action<WebClient> proxySettingsSetter = null)
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
                result.Content = webClient.DownloadString(url);
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
                    return new HtmlDownload { Content = $"WebException caught: Timeout after {timeout}", HttpStatusCode = 503 };
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
