using System.Net;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Network : INetwork
    {
        private static WebClient _webClient;

        public Network()
        {
            _webClient = new WebClient();
        }

        public string GetIpAddress()
        {
            var resp = _webClient.DownloadString("http://checkip.dyndns.com/");
            var a = resp.Split(':');
            var a2 = a[1].Substring(1);
            var a3 = a2.Split('<');
            var a4 = a3[0];

            return a4;
        }
    }
}
