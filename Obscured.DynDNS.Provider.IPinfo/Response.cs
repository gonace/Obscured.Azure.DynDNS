using Newtonsoft.Json;

namespace Obscured.DynDNS.Provider.IPInfo
{
    /// <summary>
    /// {
    ///    "ip": "217.215.208.73",
    ///    "hostname": "217-215-208-73-no13.tbcn.telia.com",
    ///    "city": "Örebro",
    ///    "region": "Örebro",
    ///    "country": "SE",
    ///    "loc": "59.2741,15.2066",
    ///    "org": "AS3301 Telia Company AB",
    ///    "postal": "700 02",
    ///    "timezone": "Europe/Stockholm",
    ///    "readme": "https://ipinfo.io/missingauth"
    /// }
    /// </summary>
    public class Response
    {
        [JsonProperty("ip")]
        public string Address { get; set; }
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("postal")]
        public string PostalCode { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("loc")]
        public string Location { get; set; }
        [JsonProperty("org")]
        public string Organization { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
}
