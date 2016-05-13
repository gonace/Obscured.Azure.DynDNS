using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ZonesCommand : BaseCommand, IZonesCommand
    {
        public ZonesCommand(IRestClient restClient, ISettings settings, IEventLogger eventLogger, IAzureHelper azureHelper)
            : base(restClient, settings, eventLogger, azureHelper)
        {
        }

        public IList<Zone> List()
        {
            try
            {
                AuthenticationResult = AzureHelper.GetAuthToken(AzureHelper.GetSubscriptionTenantId(Settings.SubscriptionId), Settings.ClientId, Settings.ClientSecret);
                RestClient.Authenticator = new JwtAuthenticator(AuthenticationResult.AccessToken);

                var request = new RestRequest(Settings.Azure.ZonesUri, Method.GET);
                request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
                request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);

                var response = RestClient.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<IList<Zone>>(JObject.Parse(response.Content).GetValue("value").ToString());

                var eventType = EventLogEntryType.Error;
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                    eventType = EventLogEntryType.FailureAudit;

                EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), eventType);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(JsonConvert.SerializeObject(ex), EventLogEntryType.Error);
            }
            return null;
        }

        public Zone Get(string name)
        {
            try
            {
                AuthenticationResult = AzureHelper.GetAuthToken(AzureHelper.GetSubscriptionTenantId(Settings.SubscriptionId), Settings.ClientId, Settings.ClientSecret);
                RestClient.Authenticator = new JwtAuthenticator(AuthenticationResult.AccessToken);

                var request = new RestRequest(Settings.Azure.ZoneUri, Method.GET);
                request.AddUrlSegment("subscriptionId", Settings.SubscriptionId);
                request.AddUrlSegment("resourceGroupName", Settings.ResourceGroup);
                request.AddUrlSegment("zoneName", Settings.ZoneName);

                var response = RestClient.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<Zone>(response.Content);

                var eventType = EventLogEntryType.Error;
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                    eventType = EventLogEntryType.FailureAudit;

                EventLogger.LogMessage(JObject.Parse(response.Content).ToString(), eventType);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(JsonConvert.SerializeObject(ex), EventLogEntryType.Error);
            }
            return null;
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
