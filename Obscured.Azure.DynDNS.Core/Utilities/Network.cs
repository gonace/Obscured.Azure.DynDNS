using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Obscured.Azure.DynDNS.Core.Models;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Network : INetwork
    {
        private static ISettings _settings;
        private static IEventLogger _eventLogger;

        public Network(ISettings settings, IEventLogger eventLogger)
        {
            _settings = settings;
            _eventLogger = eventLogger;
        }

        public List<Provider> GetIpAddress()
        {
            var tasks = _settings.Providers.Select(ProcessUrl).ToList();
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
    }
}
