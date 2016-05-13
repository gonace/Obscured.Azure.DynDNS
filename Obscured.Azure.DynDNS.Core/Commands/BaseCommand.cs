using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class BaseCommand : IBaseCommand
    {
        public static AuthenticationResult AuthenticationResult;
        public static IRestClient RestClient;
        public static ISettings Settings;
        public static IEventLogger EventLogger;
        public static IAzureHelper AzureHelper;

        public BaseCommand(IRestClient restClient, ISettings settings, IEventLogger eventLogger, IAzureHelper azureHelper)
        {
            Settings = settings;
            RestClient = restClient;
            EventLogger = eventLogger;
            AzureHelper = azureHelper;
            
            RestClient.BaseUrl = new System.Uri(Settings.Azure.BaseUri);
        }
    }
}
