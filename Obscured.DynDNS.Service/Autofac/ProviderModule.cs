using Autofac;

namespace Obscured.DynDNS.Service.Autofac
{
    public class ProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Provider.Ipify.Resolver>().As<Provider.Resolver>().SingleInstance();
            builder.RegisterType<Provider.IPInfo.Resolver>().As<Provider.Resolver>().SingleInstance();
            builder.RegisterType<Provider.MyExternalIP.Resolver>().As<Provider.Resolver>().SingleInstance();
            builder.RegisterType<Provider.MyIP.Resolver>().As<Provider.Resolver>().SingleInstance();
        }
    }
}
