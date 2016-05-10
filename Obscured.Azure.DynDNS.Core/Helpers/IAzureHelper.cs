using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public interface IAzureHelper
    {
        string GetSubscriptionTenantId(string subscriptionId);
        AuthenticationResult GetAuthToken(string tenantId, string clientId, string clientSecret);
    }
}
