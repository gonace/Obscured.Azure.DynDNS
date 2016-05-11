using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;
using RestSharp.Authenticators;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class BaseCommand : IBaseCommand
    {
        public static AuthenticationResult AuthenticationResult;
        public static IRestClient RestClient;
        public static ISettings Settings;
        public static IEventLogger EventLogger;

        public BaseCommand(IRestClient restClient, ISettings settings, IEventLogger eventLogger)
        {
            Settings = settings;
            RestClient = restClient;
            EventLogger = eventLogger;

            AuthenticationResult = new AzureHelper(Settings).GetAuthToken(new AzureHelper(Settings).GetSubscriptionTenantId(Settings.SubscriptionId), Settings.ClientId, Settings.ClientSecret);
            RestClient.Authenticator = new JwtAuthenticator(AuthenticationResult.AccessToken);
            RestClient.BaseUrl = new System.Uri(Settings.Azure.BaseUri);
        }

        protected static void RefreshToken()
        {
            AuthenticationResult = new AzureHelper(Settings).GetAuthToken(new AzureHelper(Settings).GetSubscriptionTenantId(Settings.SubscriptionId), Settings.ClientId,Settings.ClientSecret);
        }
    }
}
