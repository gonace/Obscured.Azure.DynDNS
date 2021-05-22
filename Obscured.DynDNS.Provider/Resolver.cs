using System.Net;

namespace Obscured.DynDNS.Provider
{
    public abstract class Resolver
    {
        public abstract string Url();
        public abstract IPAddress Resolve();

        protected Client Client => new Client(Url());
    }
}
