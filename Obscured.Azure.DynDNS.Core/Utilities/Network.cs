using System;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Network : INetwork
    {
        private static WebClient _webClient;
        private static IEventLogger _eventLogger;

        public Network()
        {
            _webClient = new WebClient();
            _eventLogger = new EventLogger("Azure.DynDNS");
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
                case "wtfismyip":
                    return ByWtfIsMyIp();
                default:
                    return ByDynDns();
            }
        }

        private static IPAddress ByDynDns()
        {
            try
            {
                var resp = _webClient.DownloadString("http://checkip.dyndns.com/");
                var a = resp.Split(':');
                var a2 = a[1].Substring(1);
                var a3 = a2.Split('<');
                var a4 = a3[0];
                return IPAddress.Parse(a4);
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                return IPAddress.Parse("127.0.0.1");
            }
        }

        private static IPAddress ByFreeGeoIp()
        {
            try
            {
                var resp = _webClient.DownloadString("http://freegeoip.net/json/");
                var json = JObject.Parse(resp);
                return IPAddress.Parse(json.GetValue("ip").ToString());
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                return IPAddress.Parse("127.0.0.1");
            }
        }

        private static IPAddress ByIpify()
        {
            try
            {
                var resp = _webClient.DownloadString("https://api.ipify.org/");
                return IPAddress.Parse(resp);
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                return IPAddress.Parse("127.0.0.1");
            }
        }

        private static IPAddress ByIpInfo()
        {
            try
            {
                var resp = _webClient.DownloadString("https://ipinfo.io/ip");
                return IPAddress.Parse(resp.Replace("\n", ""));
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                return IPAddress.Parse("127.0.0.1");
            }
        }

        private static IPAddress ByICanHazIp()
        {
            try
            {
                var resp = _webClient.DownloadString("https://icanhazip.com/");
                return IPAddress.Parse(resp.Replace("\n", ""));
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                return IPAddress.Parse("127.0.0.1");
            }
        }

        private static IPAddress ByWtfIsMyIp()
        {
            try
            {
                var resp = _webClient.DownloadString("https://wtfismyip.com/json");
                var json = JObject.Parse(resp);
                return IPAddress.Parse(json.GetValue("YourFuckingIPAddress").ToString());
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                return IPAddress.Parse("127.0.0.1");
            }
        }
    }
}
