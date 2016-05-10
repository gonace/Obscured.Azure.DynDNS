using System.Collections.Generic;
using Obscured.Azure.DynDNS.Core.Models;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public interface INetwork
    {
        List<Provider> GetIpAddress();
    }
}
