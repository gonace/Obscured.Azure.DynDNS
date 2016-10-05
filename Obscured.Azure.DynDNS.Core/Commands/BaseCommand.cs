using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Mindscape.Raygun4Net;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class BaseCommand : IBaseCommand
    {
        protected RaygunClient RaygunClient;
        protected static AuthenticationResult AuthenticationResult;
        protected static IRestClient RestClient;
        protected static ISettings Settings;
        protected static IEventLogger EventLogger;
        protected static IAzureHelper AzureHelper;

        public BaseCommand(IRestClient restClient, ISettings settings, IEventLogger eventLogger, IAzureHelper azureHelper)
        {
            Settings = settings;
            RestClient = restClient;
            EventLogger = eventLogger;
            AzureHelper = azureHelper;
            
            RestClient.BaseUrl = new System.Uri(Settings.Azure.BaseUri);
            RaygunClient = new RaygunClient(Settings.Obscured.RayGunApiKey);
        }
    }
}
