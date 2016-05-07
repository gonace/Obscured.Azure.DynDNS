using System.Configuration;
using Obscured.Azure.DynDNS.Core.Helpers;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Settings : ConfigurationElement, ISettings
    {
        private readonly IConfigHelper _configHelper;

        public string SubscriptionId { get; set; }
        public ObscuredSettings Obscured { get; set; }

        public Settings(IConfigHelper configHelper)
        {
            _configHelper = configHelper;
            LoadSettings();
        }

        protected void LoadSettings()
        {
            SubscriptionId = _configHelper.Get("SubscriptionId") ?? "";
            Obscured = new ObscuredSettings(_configHelper.Get("SubscriptionUri", "Obscured"), _configHelper.Get("ManagementUri", "Obscured"));
        }
    }

    public class ObscuredSettings
    {
        public string SubscriptionUri { get; set; }
        public string ManagementUri { get; set; }

        public ObscuredSettings(string subscriptionUri, string managementUri)
        {
            SubscriptionUri = subscriptionUri;
            ManagementUri = managementUri;
        }
    }
}
