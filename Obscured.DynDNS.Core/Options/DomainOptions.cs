namespace Obscured.DynDNS.Core.Options
{
    public interface IDomainOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroup { get; set; }
        public string ZoneName { get; set; }
        public string RecordName { get; set; }
        public string RecordType { get; set; }
    }

    public class DomainOptions : IDomainOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroup { get; set; }
        public string ZoneName { get; set; }
        public string RecordName { get; set; }
        public string RecordType { get; set; }
    }
}
