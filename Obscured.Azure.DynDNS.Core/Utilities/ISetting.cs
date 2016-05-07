namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public interface ISettings
    {
        string SubscriptionId { get; set; }
        ObscuredSettings Obscured { get; set; }
    }
}
