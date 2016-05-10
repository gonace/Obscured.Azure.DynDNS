using System.Configuration;

namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public interface IConfigHelper
    {
        string Get(string key);
        string Get(string key, string section);

        AppSettingsSection GetSection(string section);
    }
}
