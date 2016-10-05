using System;
using System.ServiceProcess;
using Mindscape.Raygun4Net;
using Ninject;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using Obscured.Azure.DynDNS.Service.Helpers;
using Obscured.Azure.DynDNS.Service.Ninject;

namespace Obscured.Azure.DynDNS.Service
{
    static class Program
    {
        private static readonly RaygunClient RaygunClient;

        static Program()
        {
            var settings = new Settings(new ConfigHelper());
            RaygunClient = new RaygunClient(settings.Obscured.RayGunApiKey);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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

        #region Exception Handling
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            RaygunClient.Send(e.ExceptionObject as Exception);
        }
        #endregion
    }
}