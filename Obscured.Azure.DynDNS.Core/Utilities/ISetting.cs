using System.Collections.Generic;
using Obscured.Azure.DynDNS.Core.Models;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public interface ISettings
    {
        AzureSettings Azure { get; set; }
        ObscuredSettings Obscured { get; set; }
        List<Provider> Providers { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string RecordName { get; set; }
        string RecordType { get; set; }
        string ResourceGroup { get; set; }
        string SubscriptionId { get; set; }
        int PoolingInterval { get; set; }
        string ZoneName { get; set; }
    }
}
