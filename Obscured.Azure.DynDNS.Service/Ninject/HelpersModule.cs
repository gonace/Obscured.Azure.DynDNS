﻿using Ninject.Modules;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Service.Helpers;

namespace Obscured.Azure.DynDNS.Service.Ninject
{
    public class HelpersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigHelper>().To<ConfigHelper>().InTransientScope();
            Bind<IAzureHelper>().To<AzureHelper>().InTransientScope();
            Bind<RegistryHelper>().To<RegistryHelper>().InTransientScope();
            Bind<IServiceHelper>().To<ServiceHelper>().InTransientScope();
        }
    }
}
