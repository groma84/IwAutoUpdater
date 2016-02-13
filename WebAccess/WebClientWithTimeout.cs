using System;
using System.Net;

namespace IwAutoUpdater.DAL.WebAccess
{
    public class WebClientWithTimeout : WebClient
    {
        int _timeout = -1;

        public WebClientWithTimeout(TimeSpan timeout)
        {
            _timeout = (int)timeout.TotalMilliseconds;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);
            webRequest.Timeout = _timeout;
            return webRequest;
        }
    }
}
