using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models.Zones
{
    public class Properties
    {
        public int numberOfRecordSets { get; set; }
        public int maxNumberOfRecordSets { get; set; }
        public IList<string> nameServers { get; set; }
        public string parentResourceGroupName { get; set; }
    }
}
