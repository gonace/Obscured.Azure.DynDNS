using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            var serviceProviders = new Models.Services.ServiceProviders();
            var tasks = serviceProviders.Providers.Select(ProcessUrl).ToList();
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException) {}

            return tasks.Select(t => t.Result).ToList();
        }


        private static async Task<Provider> ProcessUrl(Provider provider)
        {
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(new Uri(provider.URL)))
            using (var content = response.Content)
            {
                var result = await content.ReadAsStringAsync();
                var contentType = response.Content.Headers.ContentType;

                if (!response.IsSuccessStatusCode) return null;

                if (Equals(contentType.MediaType, "application/json"))
                {
                    var json = JObject.Parse(result);
                    provider.ReturnedAddress = json.GetValue("ip").ToString();
                    return provider;
                }
                else if (Equals(contentType.MediaType, "text/html"))
                {
                    throw new NotImplementedException("You're using a provider that have an text/html response, this content-type is not supported.");
                }

                throw new NotImplementedException($"You're using a provider that have an unsuported content-type {contentType.MediaType} response.");
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
                return IPAddress.Parse(resp.Replace("\n",""));
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
