namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public interface IAzureHelper
    {
        string GetSubscriptionTenantId(string subscriptionId);
        string GetAuthToken(string tenantId, string clientId, string clientSecret);
    }
}
