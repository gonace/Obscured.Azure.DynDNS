using Ninject.Modules;
using Obscured.Azure.DynDNS.Core.Commands;

namespace Obscured.Azure.DynDNS.Service.Ninject
{
    public class CommandsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBaseCommand>().To<BaseCommand>().InTransientScope();
            Bind<IRecordsCommand>().To<RecordsCommand>().InTransientScope();
            Bind<IZonesCommand>().To<ZonesCommand>().InTransientScope();
        }
    }
}
