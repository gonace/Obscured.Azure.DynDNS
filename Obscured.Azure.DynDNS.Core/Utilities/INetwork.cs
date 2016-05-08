using System.Net;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public interface INetwork
    {
        IPAddress GetIpAddress(string provider = "dyndns");
    }
}
