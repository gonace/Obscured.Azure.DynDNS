using System.Net;

namespace Obscured.DynDNS.Provider.MyExternalIP
{
    public class Resolver : Provider.Resolver
    {
        public override IPAddress Resolve()
        {
            var response = Client.ResolveAsync<Response>().Result;

            return IPAddress.Parse(response.Address);
        }

        public override string Url()
        {
            return "https://myexternalip.com/json";
        }
    }
}
