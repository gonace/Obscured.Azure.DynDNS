using Ninject.Modules;
using Obscured.Azure.DynDNS.Core.Utilities;

namespace Obscured.Azure.DynDNS.Service.Ninject
{
    public class UtilitiesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>().InTransientScope();
            Bind<INetwork>().To<Network>().InTransientScope();
            Bind<IEventLogger>().To<EventLogger>().InTransientScope().WithConstructorArgument("sourceName", "Azure.DynDNS");
        }
    }
}
