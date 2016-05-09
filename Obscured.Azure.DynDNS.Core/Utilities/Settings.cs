using System;
using System.Configuration;
using Obscured.Azure.DynDNS.Core.Helpers;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Settings : ConfigurationElement, ISettings
    {
        private readonly IConfigHelper _configHelper;

        public AzureSettings Azure { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RecordName { get; set; }
        public string RecordType { get; set; }
        public string ResourceGroup { get; set; }
        public string SubscriptionId { get; set; }
        public string Providers { get; set; }
        public int PoolingInterval { get; set; }
        public string ZoneName { get; set; }

        public Settings(IConfigHelper configHelper)
        {
            _configHelper = configHelper;
            LoadSettings();
        }

        protected void LoadSettings()
        {
            ClientId = _configHelper.Get("ClientId") ?? "";
            ClientSecret = _configHelper.Get("ClientSecret") ?? "";
            RecordName = _configHelper.Get("RecordName") ?? "";
            RecordType = _configHelper.Get("RecordType") ?? "";
            ResourceGroup = _configHelper.Get("ResourceGroup") ?? "";
            SubscriptionId = _configHelper.Get("SubscriptionId") ?? "";
            Providers = _configHelper.Get("Providers") ?? "";
            PoolingInterval = Convert.ToInt32(_configHelper.Get("PoolingInterval"));
            ZoneName = _configHelper.Get("ZoneName") ?? "";

            Azure = new AzureSettings
            {
                ManagementUri = _configHelper.Get("ManagementUri", "Azure"),
                BaseUri = _configHelper.Get("BaseUri", "Azure"),
                SubscriptionsUri = _configHelper.Get("SubscriptionsUri", "Azure"),
                ZonesUri = _configHelper.Get("ZonesUri", "Azure"),
                ZoneUri = _configHelper.Get("ZoneUri", "Azure"),
                RecordsUri = _configHelper.Get("RecordsUri", "Azure"),
                RecordUri = _configHelper.Get("RecordUri", "Azure")
            };
        }
    }

    public class AzureSettings
    {
        public string ManagementUri { get; set; }
        public string BaseUri { get; set; }
        public string SubscriptionsUri { get; set; }
        public string ZonesUri { get; set; }
        public string ZoneUri { get; set; }
        public string RecordsUri { get; set; }
        public string RecordUri { get; set; }

        public AzureSettings()
        {
        }
    }
}
