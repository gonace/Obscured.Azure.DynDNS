using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Obscured.Azure.DynDNS.Core.Utilities;

namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public class AzureHelper : IAzureHelper
    {
        private readonly ISettings _settings;
        private readonly IEventLogger _eventLogger;

        public AzureHelper(ISettings settings, IEventLogger eventLogger)
        {
            _settings = settings;
            _eventLogger = eventLogger;
        }

        public string GetSubscriptionTenantId(string subscriptionId)
        {
            //  URL to Azure Resource Manager API
            var url = string.Format(_settings.Azure.ManagementUri, subscriptionId);

            // Attempting an ARM request without JWT will return an auth discovery header
            AuthenticationHeaderValue wwwAuthenticateHeader;
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(url).Result;
                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    throw new InvalidOperationException("Failure requesting subscription tenant id from ARM.");
                }

                wwwAuthenticateHeader = response.Headers.WwwAuthenticate.Single();
            }

            //  chop up the response
            var tokens = wwwAuthenticateHeader.Parameter.Split('=', ',');
            var authUrl = tokens.ElementAt(1).Trim('"');
            var tenantId = new Uri(authUrl).AbsolutePath.Trim('/');
            return tenantId;
        }
        /*
         *   Get an auth token for a given tenant ID (from GetSubscriptionTenantId)
         *   userId specifies the expected user, if no userId is specified alwaysPrompt=false will use current user if already logged in
         */
        public AuthenticationResult GetAuthToken(string tenantId, string clientId, string clientSecret)
        {
            try
            {
                /* var authContext = new AuthenticationContext($"https://login.windows.net/{tenantId}");
                 var credential = new ClientCredential(clientId, clientSecret);

                 //var result = authContext.AcquireTokenAsync(resource: "https://management.core.windows.net/{0}", clientCredential: credential);
                 var task = authContext.AcquireTokenAsync("https://management.core.windows.net/", credential);
                 Task.WhenAny(Task.WhenAll(task), Task.Delay(60000));

                 var result = task.Result;
                 if (result == null)
                 {
                     throw new InvalidOperationException("Failed to obtain the JWT token");
                 }
                 return result;*/

                var context = new AuthenticationContext($"https://login.windows.net/{tenantId}");
                var clientCred = new ClientCredential(clientId, clientSecret);

                var task = context.AcquireTokenAsync("https://management.core.windows.net/", clientCred);
                Task.WhenAny(Task.WhenAll(task), Task.Delay(60000));

                var result = task.Result;
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to obtain the JWT token");
                }
                return result;
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(JsonConvert.SerializeObject(ex), EventLogEntryType.Error);
            }
            return null;
        }
    }
}
