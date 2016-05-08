using System.ServiceProcess;
using Ninject;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using Obscured.Azure.DynDNS.Service.Ninject;

namespace Obscured.Azure.DynDNS.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var kernel = new StandardKernel(new HelpersModule(), new UtilitiesModule());

            var settings = kernel.Get<ISettings>();
            var network = kernel.Get<INetwork>();
            var azureHelper = kernel.Get<IAzureHelper>();


            var servicesToRun = new ServiceBase[] 
            { 
                new DynDns(settings, network, azureHelper) 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
