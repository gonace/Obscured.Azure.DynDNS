using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obscured.Azure.DynDNS.Core.Models;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class RecordsCommand : BaseCommand, IRecordsCommand
    {
        public RecordsCommand(IRestClient restClient, ISettings settings, IEventLogger eventLogger)
            : base(restClient, settings, eventLogger)
        {
        }

        public IList<Record> List()
        {
            if (DateTimeOffset.UtcNow > AuthenticationResult.ExpiresOn)
                ReAuthenticate();

            var request = new RestRequest(Settings.Azure.RecordsUri, Method.GET);
            request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);
            request.AddUrlSegment("zoneName", Settings.ZoneName);

            var response = RestClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<Record>>(JObject.Parse(response.Content).GetValue("value").ToString());

            EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), EventLogEntryType.Error);
            return null;
        }

        public Record Get(string name, string type = "")
        {
            if (DateTimeOffset.UtcNow > AuthenticationResult.ExpiresOn)
                ReAuthenticate();

            var request = new RestRequest(Settings.Azure.RecordUri, Method.GET);
            request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);
            request.AddUrlSegment("zoneName", Settings.ZoneName);
            request.AddUrlSegment("recordType", type);
            request.AddUrlSegment("recordSetName", name);

            var response = RestClient.Execute(request);
            if(response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Record>(response.Content);
            
            EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), EventLogEntryType.Error);
            return null;
        }

        public Record Create(Record record, string zoneName)
        {
            if (DateTimeOffset.UtcNow > AuthenticationResult.ExpiresOn)
                ReAuthenticate();

            var request = new RestRequest(Settings.Azure.RecordUri, Method.PUT) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);
            request.AddUrlSegment("zoneName", zoneName);
            request.AddUrlSegment("recordType", Settings.RecordType);
            request.AddUrlSegment("recordSetName", record.name);
            var jsonRecord = JsonConvert.SerializeObject(record, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            request.AddParameter("application/json", jsonRecord, ParameterType.RequestBody);

            var response = RestClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Record>(response.Content);

            EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), EventLogEntryType.Error);
            return null;
        }

        public Record Update(Record record, string zoneName)
        {
            if (DateTimeOffset.UtcNow > AuthenticationResult.ExpiresOn)
                ReAuthenticate();

            var request = new RestRequest(Settings.Azure.RecordUri, Method.PUT)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new ObscuredJsonSerializer(new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                })
            };
            request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);
            request.AddUrlSegment("zoneName", zoneName);
            request.AddUrlSegment("recordType", Settings.RecordType);
            request.AddUrlSegment("recordSetName", record.name);
            var newRecord = new Record
            {
                location = record.location,
                tags = record.tags,
                properties = record.properties
            };
            newRecord.properties.fqdn = null;
            request.AddBody(newRecord);

            var response = RestClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Record>(response.Content);

            EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), EventLogEntryType.Error);
            return null;
        }

        public bool Remove(Record record)
        {
            if (DateTimeOffset.UtcNow > AuthenticationResult.ExpiresOn)
                ReAuthenticate();

            var request = new RestRequest(Settings.Azure.RecordUri, Method.DELETE);
            request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);
            request.AddUrlSegment("zoneName", Settings.ZoneName);
            request.AddUrlSegment("recordType", Settings.RecordType);
            request.AddUrlSegment("recordSetName", record.name);

            var response = RestClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return response.StatusCode == HttpStatusCode.OK;

            EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), EventLogEntryType.Error);
            return false;
        }
    }
}
