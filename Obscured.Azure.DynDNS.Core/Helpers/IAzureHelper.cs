namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public interface IAzureHelper
    {
        string GetSubscriptionTenantId(string subscriptionId);
        string GetAuthToken(string tenantId, bool alwaysPrompt = false, string userId = null);
    }
}
