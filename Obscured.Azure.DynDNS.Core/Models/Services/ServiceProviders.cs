using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models.Services
{
    public class ServiceProviders
    {
        public IList<Provider> Providers { get; set; }

        public ServiceProviders()
        {
            Providers = new List<Provider>
            {
                new Provider {Name = "Obscured", URL = "http://www.holdr.se/utility/ip"},
                new Provider {Name = "FreeGeoIP", URL = "http://freegeoip.net/json/"},
                new Provider {Name = "Ipify", URL = "https://api.ipify.org?format=json"},
                new Provider {Name = "IPInfo", URL = "http://ipinfo.io/json"},
                new Provider {Name = "MyExternalIP", URL = "http://myexternalip.com/json"}
            };
        }
    }
}
