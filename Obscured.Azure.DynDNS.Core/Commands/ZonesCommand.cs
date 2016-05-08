using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Models;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;
using RestSharp.Authenticators;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class ZonesCommand : BaseCommand, IZonesCommand
    {
        private readonly RestClient _restClient;
        private readonly Settings _settings;

        public ZonesCommand(RestClient restClient, Settings settings) : base(restClient, settings)
        {
            _restClient = restClient;
            _settings = settings;
            
            var jwToken = new AzureHelper(settings).GetAuthToken(
                new AzureHelper(settings).GetSubscriptionTenantId(_settings.SubscriptionId), _settings.ClientId,
                _settings.ClientSecret);
            _restClient.Authenticator = new JwtAuthenticator(jwToken);
            _restClient.BaseUrl = new System.Uri(_settings.Azure.BaseUri);
        }

        public IList<Zone> List()
        {
            var request = new RestRequest(_settings.Azure.ZonesUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<IList<Zone>>(JObject.Parse(response.Content).GetValue("value").ToString());
        }

        public Zone Get(string name)
        {
            var request = new RestRequest(_settings.Azure.ZoneUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", _settings.ZoneName);
            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<Zone>(response.Content);
        }

        public Zone Create(Zone zone)
        {
            throw new System.NotImplementedException();
        }

        public Zone Update(Zone zone)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(Zone zone)
        {
            throw new System.NotImplementedException();
        }
    }
}
