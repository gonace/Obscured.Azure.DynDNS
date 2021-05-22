using System.Collections.Generic;
using Newtonsoft.Json;

namespace Obscured.DynDNS.Client.Models.Zone
{
    public class Zone
    {
        [JsonProperty("numberOfRecordSets")]
        public int NumberOfRecordSets { get; set; }
        [JsonProperty("maxNumberOfRecordSets")]
        public int MaxNumberOfRecordSets { get; set; }
        [JsonProperty("nameServers")]
        public IList<string> NameServers { get; set; }
        [JsonProperty("parentResourceGroupName")]
        public string ParentResourceGroupName { get; set; }
    }
}
