using System.ServiceProcess;
using Ninject;
using Obscured.Azure.DynDNS.Core.Utilities;
using Obscured.Azure.DynDNS.Service.Helpers;
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
            var kernel = new StandardKernel(new HelpersModule(), new UtilitiesModule(), new CommandsModule());
            var settings = kernel.Get<ISettings>();
            var serviceHelper = kernel.Get<IServiceHelper>();
            var eventLogger = kernel.Get<IEventLogger>();

            var servicesToRun = new ServiceBase[]
            { 
                new DynDns(settings, serviceHelper, eventLogger) 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
