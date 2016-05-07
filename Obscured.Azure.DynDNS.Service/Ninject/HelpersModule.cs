using Ninject.Modules;
using Obscured.Azure.DynDNS.Core.Helpers;

namespace Obscured.Azure.DynDNS.Service.Ninject
{
    public class HelpersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigHelper>().To<ConfigHelper>().InTransientScope();
            Bind<IAzureHelper>().To<AzureHelper>().InTransientScope();
        }
    }
}
