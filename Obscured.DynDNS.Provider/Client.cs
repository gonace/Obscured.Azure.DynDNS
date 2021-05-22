using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Obscured.DynDNS.Provider
{
    public class Client
    {
        private readonly string _url;

        public Client(string url)
        {
            _url = url;
        }

        public async Task<TReturn> ResolveAsync<TReturn>()
        {
            using (var client = GetHttpClient())
            {
                var response = await client.GetAsync(_url);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<TReturn>(await response.Content.ReadAsStringAsync());

                //var details = response?.Content != null ? JsonConvert.DeserializeObject<laget.Exceptions.Models.Response>(await response.Content.ReadAsStringAsync()) : null;
                //throw new Exception(details?.Title, details?.Status, details);

                throw new Exception(await response.Content.ReadAsStringAsync());
            }

            //using (var httpClient = new HttpClient())
            //using (var response = await httpClient.GetAsync(new Uri(provider.URL)))
            //using (var content = response.Content)
            //{
            //    var result = await content.ReadAsStringAsync();
            //    var contentType = response.Content.Headers.ContentType;

            //    if (!response.IsSuccessStatusCode) return null;

            //    if (Equals(contentType.MediaType, "application/json"))
            //    {
            //        var json = JObject.Parse(result);
            //        provider.ReturnedAddress = json.GetValue("ip").ToString();
            //        return provider;
            //    }
            //    else if (Equals(contentType.MediaType, "text/html"))
            //    {
            //        throw new NotImplementedException("You're using a provider that have an text/html response, this content-type is not supported.");
            //    }

            //    throw new NotImplementedException($"You're using a provider that have an unsuported content-type {contentType.MediaType} response.");
            //}
        }


        private HttpClient GetHttpClient()
        {
            var apiClient = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };

            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return apiClient;
        }
    }
}
