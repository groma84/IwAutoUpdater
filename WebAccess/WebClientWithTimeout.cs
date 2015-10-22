using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
