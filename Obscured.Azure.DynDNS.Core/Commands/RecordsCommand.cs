using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Models;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;
using RestSharp.Authenticators;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class RecordsCommand : BaseCommand, IRecordsCommand
    {
        private readonly RestClient _restClient;
        private readonly Settings _settings;

        public RecordsCommand(RestClient restClient, Settings settings)
            : base(restClient, settings)
        {
            _restClient = restClient;
            _settings = settings;

            var jwToken = new AzureHelper(settings).GetAuthToken(
                new AzureHelper(settings).GetSubscriptionTenantId(_settings.SubscriptionId), _settings.ClientId,
                _settings.ClientSecret);
            _restClient.Authenticator = new JwtAuthenticator(jwToken);
            _restClient.BaseUrl = new System.Uri(_settings.Azure.BaseUri);
        }

        public IList<Record> List()
        {
            var request = new RestRequest(_settings.Azure.RecordsUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", _settings.ZoneName);
            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<IList<Record>>(JObject.Parse(response.Content).GetValue("value").ToString());
        }

        public Record Get(string name, string type = "A")
        {
            var request = new RestRequest(_settings.Azure.RecordUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", _settings.ZoneName);
            request.AddUrlSegment("recordType", type);
            request.AddUrlSegment("recordSetName", name);
            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<Record>(response.Content);
        }
        
        public Record Create(Record record, string zoneName)
        {
            var request = new RestRequest(_settings.Azure.RecordUri, Method.PUT);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", zoneName);
            request.AddUrlSegment("recordType", record.Type);
            request.AddUrlSegment("recordSetName", record.Name);

            var jsonRecord = JsonConvert.SerializeObject(record, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            request.AddBody(jsonRecord);

            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<Record>(response.Content);
        }

        public Record Update(Record record, string zoneName)
        {
            var request = new RestRequest(_settings.Azure.RecordUri, Method.PUT);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", zoneName);
            request.AddUrlSegment("recordType", record.Type);
            request.AddUrlSegment("recordSetName", record.Name);

            var jsonRecord = JsonConvert.SerializeObject(record, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            request.AddBody(jsonRecord);

            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<Record>(response.Content);
        }

        public bool Remove(Record record)
        {
            var request = new RestRequest(_settings.Azure.RecordUri, Method.DELETE);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", _settings.ZoneName);
            request.AddUrlSegment("recordType", record.Type);
            request.AddUrlSegment("recordSetName", record.Name);
            var response = _restClient.Execute(request);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
