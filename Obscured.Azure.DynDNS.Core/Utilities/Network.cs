using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json.Linq;
using Obscured.Azure.DynDNS.Core.Models;

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

        public List<Provider> GetIpAddress(string providers = "dyndns")
        {
            var providersArr = providers.Split(',');
            var providersResult = new List<Provider>();

            foreach (var provider in providersArr)
            {
                IPAddress returnedAddress;
                switch (provider)
                {
                    case "dyndns":
                        returnedAddress = ByDynDns();
                        break;
                    case "freegeoip":
                        returnedAddress = ByFreeGeoIp();
                        break;
                    case "ipify":
                        returnedAddress = ByIpify();
                        break;
                    case "ipinfo":
                        returnedAddress = ByIpInfo();
                        break;
                    case "icanhazip":
                        returnedAddress = ByICanHazIp();
                        break;
                    case "wtfismyip":
                        returnedAddress = ByWtfIsMyIp();
                        break;
                    default:
                        returnedAddress = ByDynDns();
                        break;
                }

                providersResult.Add(new Provider
                {
                    Name = provider,
                    ReturnedAddress = returnedAddress.ToString()
                });
            }

            return providersResult;
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
