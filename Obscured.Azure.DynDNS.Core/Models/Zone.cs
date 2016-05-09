using System;
using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models
{
    public class Zone
    {
        public string id { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public IDictionary<string, string> tags { get; set; }
        public string type { get; set; }
        public Guid? etag { get; set; }
        public Zones.Properties properties { get; set; }
    }
}
