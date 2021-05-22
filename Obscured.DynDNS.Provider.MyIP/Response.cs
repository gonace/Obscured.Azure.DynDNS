using Newtonsoft.Json;

namespace Obscured.DynDNS.Provider.MyIP
{
    /// <summary>
    /// {
    ///   "success": true,
    ///   "ip": "217.215.208.73",
    ///   "type": "IPv4"
    /// }
    /// </summary>
    public class Response
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("ip")]
        public string Address { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
