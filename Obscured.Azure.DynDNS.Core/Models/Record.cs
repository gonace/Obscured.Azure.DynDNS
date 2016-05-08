using System;

namespace Obscured.Azure.DynDNS.Core.Models
{
    public class Record
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid Etag { get; set; }
        public string Location { get; set; }
        public Records.Properties Properties { get; set; }
    }
}
