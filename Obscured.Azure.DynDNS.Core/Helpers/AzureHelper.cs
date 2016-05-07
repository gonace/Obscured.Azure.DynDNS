using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Obscured.Azure.DynDNS.Core.Utilities;

namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public class AzureHelper : IAzureHelper
    {
        private readonly ISettings _settings;

        public AzureHelper(ISettings settings)
        {
            _settings = settings;
        }

        public string GetSubscriptionTenantId(string subscriptionId)
        {
            //  URL to Azure Resource Manager API
            var url = string.Format(_settings.Obscured.SubscriptionUri, subscriptionId);

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
        public string GetAuthToken(string tenantId, bool alwaysPrompt = false, string userId = null)
        {
            AuthenticationResult authResult;
            try
            {
                authResult = GetAuthResult(tenantId, alwaysPrompt, userId);
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("returned by service does not match user"))
                {
                    Console.WriteLine("Got username exception, trying that again without username...");
                    // if it complained about username provided, try without it
                    authResult = GetAuthResult(tenantId, true, null);
                }
                else
                {
                    throw e;
                }
            }

            return authResult == null ? null : authResult.CreateAuthorizationHeader().Substring("Bearer ".Length);
        }

        public AuthenticationResult GetAuthResult(string tenantId, bool alwaysPrompt, string userId)
        {
            var context = new AuthenticationContext(string.Format("https://login.windows.net/{0}", tenantId));

            Task<AuthenticationResult> acquireTokenTask;
            if (!string.IsNullOrEmpty(userId))
            {
                acquireTokenTask = context.AcquireTokenAsync(
                    resource: _settings.Obscured.ManagementUri,
                    clientId: "1950a258-227b-4e31-a9cf-717495945fc2",
                    redirectUri: new Uri("urn:ietf:wg:oauth:2.0:oob"),
                    parameters: new PlatformParameters(promptBehavior: alwaysPrompt ? PromptBehavior.Always : PromptBehavior.Auto, ownerWindow: null),
                    userId: new UserIdentifier(userId, UserIdentifierType.RequiredDisplayableId));
            }
            else
            {
                acquireTokenTask = context.AcquireTokenAsync(
                    resource: _settings.Obscured.ManagementUri,
                    clientId: "1950a258-227b-4e31-a9cf-717495945fc2",
                    redirectUri: new Uri("urn:ietf:wg:oauth:2.0:oob"),
                    parameters: new PlatformParameters(alwaysPrompt ? PromptBehavior.Always : PromptBehavior.Auto, ownerWindow: null));
            }

            try
            {
                acquireTokenTask.Wait();
                return acquireTokenTask.Result;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
