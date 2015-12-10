using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

namespace Asmodat.Networking
{
    public partial class WebClients
    {

        public static string DownloadString(string url)
        {
            return WebClients.DownloadString(url, null, null, null, null, null);
        }

        public static string DownloadString(string url, string proxy, string proxyUser, string proxyPassword, bool? useDefaultCredentials, bool? bypassProxyOnLocal)
        {
            string result = null;

            using (WebClient client = new WebClient())
            {
                WebProxy wproxy = new WebProxy();

                if (!proxy.IsNullOrWhiteSpace())
                {
                    wproxy.Address = new Uri(proxy);
                }

                if (!proxyUser.IsNullOrWhiteSpace() && !proxyPassword.IsNullOrWhiteSpace())
                {
                    wproxy.Credentials = new NetworkCredential(proxyUser, proxyPassword);

                    if (useDefaultCredentials != null)
                        wproxy.UseDefaultCredentials = useDefaultCredentials.Value;
                }

                if(bypassProxyOnLocal != null)
                    wproxy.BypassProxyOnLocal = bypassProxyOnLocal.Value;

                client.Proxy = wproxy;
             

                result = client.DownloadString(url);
            }

            return result;
        }


      

    }
}
