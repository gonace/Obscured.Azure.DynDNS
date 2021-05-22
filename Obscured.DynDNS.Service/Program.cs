using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using laget.Quartz.Extensions;
using Microsoft.Extensions.Hosting;
using Obscured.DynDNS.Service.Autofac;
using Serilog;

namespace Obscured.DynDNS.Service
{
    internal static class Program
    {
        private static async Task Main()
        {
            await Host.CreateDefaultBuilder()
                .ConfigureContainer<ContainerBuilder>((context, builder) =>
                {
                    builder.RegisterModule<OptionsModule>();
                    builder.RegisterModule<ProviderModule>();
                    builder.RegisterQuartz();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddQuartzService();
                })
                .UseConsoleLifetime()
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
                .UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseWindowsService()
                .Build()
                .RunAsync();
        }
    }
}