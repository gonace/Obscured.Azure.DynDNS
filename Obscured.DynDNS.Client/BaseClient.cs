using Microsoft.Azure.Management.Dns.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace Obscured.DynDNS.Client
{
    public class BaseClient
    {
        protected readonly IDnsZone DnsZone;
        protected readonly IResourceGroup ResourceGroup;

        public BaseClient(IOptions options)
        {
            var credentials = new AzureCredentialsFactory().FromServicePrincipal(options.ClientId, options.ClientSecret, options.TenantId, AzureEnvironment.AzureGlobalCloud);
            var azure = Azure.Authenticate(credentials).WithDefaultSubscription();

            ResourceGroup = azure.ResourceGroups
                .Define(options.ResourceGroup)
                .WithRegion(options.Region)
                .Create();

            DnsZone = azure.DnsZones
                .Define(options.ZoneName)
                .WithExistingResourceGroup(ResourceGroup)
                .Create();
        }
    }
}
