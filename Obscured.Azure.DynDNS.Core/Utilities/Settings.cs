using System;
using System.Collections.Generic;
using System.Configuration;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Models;
using System.Diagnostics;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class Settings : ConfigurationElement, ISettings
    {
        private readonly IConfigHelper _configHelper;

        public AzureSettings Azure { get; set; }
        public ObscuredSettings Obscured { get; set; }
        public List<Provider> Providers { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RecordName { get; set; }
        public string RecordType { get; set; }
        public string ResourceGroup { get; set; }
        public string SubscriptionId { get; set; }
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

            Obscured = new ObscuredSettings
            {
                LogLevel = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), _configHelper.Get("LogLevel", "Obscured"), true),
                LogName = _configHelper.Get("LogName", "Obscured"),
                RayGunApiKey = _configHelper.Get("RayGun.ApiKey", "Obscured")
            };

            Providers = new List<Provider>();
            var providerSection = _configHelper.GetSection("Providers");
            foreach (KeyValueConfigurationElement provider in providerSection.Settings)
            {
                Providers.Add(new Provider
                {
                    Name = provider.Key,
                    URL = provider.Value
                });
            }
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
    }

    public class ObscuredSettings
    {
        public EventLogEntryType LogLevel { get; set; }
        public string LogName { get; set; }
        public string RayGunApiKey { get; set; }
    }
}
