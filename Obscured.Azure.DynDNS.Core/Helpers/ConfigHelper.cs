using System.Configuration;

namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string Get(string key, string section)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSection = (AppSettingsSection)config.GetSection(section);
            return appSection.Settings[key].Value;
        }
    }
}
