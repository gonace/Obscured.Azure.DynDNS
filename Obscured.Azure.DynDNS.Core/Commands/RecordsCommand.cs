using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Models;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Newtonsoft.Json;
using RestSharp.Serializers;
using RestRequest = RestSharp.RestRequest;

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

        public Record Get(string name, string type = "")
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
            var request = new RestRequest(_settings.Azure.RecordUri, Method.PUT) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", zoneName);
            request.AddUrlSegment("recordType", _settings.RecordType);
            request.AddUrlSegment("recordSetName", record.name);

            var jsonRecord = JsonConvert.SerializeObject(record, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            request.AddParameter("application/json", jsonRecord, ParameterType.RequestBody);

            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<Record>(response.Content);
        }

        public Record Update(Record record, string zoneName)
        {
            var request = new RestRequest(_settings.Azure.RecordUri, Method.PUT)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new ObscuredJsonSerializer(new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                })
            };

            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", zoneName);
            request.AddUrlSegment("recordType", _settings.RecordType);
            request.AddUrlSegment("recordSetName", record.name);

            var newRecord = new Record
            {
                location = record.location,
                tags = record.tags,
                properties = record.properties
            };
            newRecord.properties.fqdn = null;

            /*var jsonRecord = JsonConvert.SerializeObject(record,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            request.AddBody(jsonRecord);*/

            request.AddBody(newRecord);

            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<Record>(response.Content);
        }

        public bool Remove(Record record)
        {
            var request = new RestRequest(_settings.Azure.RecordUri, Method.DELETE);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", _settings.ZoneName);
            request.AddUrlSegment("recordType", _settings.RecordType);
            request.AddUrlSegment("recordSetName", record.name);
            var response = _restClient.Execute(request);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
