using System.Collections.Generic;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Obscured.DynDNS.Service.Autofac
{
    public class OptionsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<IConfiguration>().GetSection("Azure").Get<Client.Options>()).As<Client.IOptions>().SingleInstance();
            builder.Register(c => c.Resolve<IConfiguration>().GetSection("Domains").Get<IEnumerable<Client.Options>>()).As<IEnumerable<Client.Options>>().SingleInstance();
        }
    }
}
