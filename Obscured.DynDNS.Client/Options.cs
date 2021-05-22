
namespace Obscured.DynDNS.Client
{
    public interface IOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }

        public string Region { get; set; }
        public string ResourceGroup { get; set; }

        public string ZoneName { get; set; }
        public string RecordName { get; set; }
    }

    public class Options : IOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }

        public string Region { get; set; }
        public string ResourceGroup { get; set; }

        public string ZoneName { get; set; }
        public string RecordName { get; set; }
    }
}
