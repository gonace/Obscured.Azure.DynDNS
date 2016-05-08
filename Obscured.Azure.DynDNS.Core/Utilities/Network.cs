using System.Net;
using Newtonsoft.Json.Linq;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Network : INetwork
    {
        private static WebClient _webClient;

        public Network()
        {
            _webClient = new WebClient();
        }

        public IPAddress GetIpAddress(string provider = "dyndns")
        {
            switch (provider)
            {
                case "dyndns":
                    return ByDynDns();
                case "freegeoip":
                    return ByFreeGeoIp();
                case "ipify":
                    return ByIpify();
                case "ipinfo":
                    return ByIpInfo();
                case "icanhazip":
                    return ByICanHazIp();
                default:
                    return ByDynDns();
            }
        }

        private static IPAddress ByDynDns()
        {
            var resp = _webClient.DownloadString("http://checkip.dyndns.com/");
            var a = resp.Split(':');
            var a2 = a[1].Substring(1);
            var a3 = a2.Split('<');
            var a4 = a3[0];

            return IPAddress.Parse(a4);
        }

        private static IPAddress ByFreeGeoIp()
        {
            var resp = _webClient.DownloadString("http://freegeoip.net/json/");
            var json = JObject.Parse(resp);

            return IPAddress.Parse(json.GetValue("ip").ToString());
        }

        private static IPAddress ByIpify()
        {
            var resp = _webClient.DownloadString("https://api.ipify.org/");

            return IPAddress.Parse(resp);
        }

        private static IPAddress ByIpInfo()
        {
            var resp = _webClient.DownloadString("https://ipinfo.io/ip");

            return IPAddress.Parse(resp.Replace("\n", ""));
        }

        private static IPAddress ByICanHazIp()
        {
            var resp = _webClient.DownloadString("https://icanhazip.com/");

            return IPAddress.Parse(resp.Replace("\n", ""));
        }
    }
}
