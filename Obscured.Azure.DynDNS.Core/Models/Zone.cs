using System;
using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models
{
    public class Zone
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Tags { get; set; }
        public string Type { get; set; }
        public Guid Etag { get; set; }
        public Zones.Properties Properties { get; set; }
    }
}
