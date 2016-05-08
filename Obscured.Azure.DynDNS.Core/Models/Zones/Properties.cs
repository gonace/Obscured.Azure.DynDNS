using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models.Zones
{
    public class Properties
    {
        public int MaxNumberOfRecordSets { get; set; }
        public IList<string> NameServers { get; set; }
        public int NumberOfRecordSets { get; set; }
        public string ParentResourceGroupName { get; set; }
    }
}
