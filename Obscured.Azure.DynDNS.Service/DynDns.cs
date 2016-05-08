using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using Obscured.Azure.DynDNS.Core.Commands;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using Obscured.Azure.DynDNS.Service.Helpers;

namespace Obscured.Azure.DynDNS.Service
{
    public partial class DynDns : ServiceBase
    {
        private static Timer _appTimer;
        private static ISettings _settings;
        private static INetwork _networkHelper;
        private static IEventLogger _eventLogger;
        private static IAzureHelper _azureHelper;
        private static IServiceHelper _serviceHelper;
        private static IRecordsCommand _recordsCommand;
        private static IZonesCommand _zonesCommand;

        public DynDns(ISettings settings, INetwork network, IAzureHelper azureHelper, IServiceHelper serviceHelper, IEventLogger eventLogger, IRecordsCommand recordsCommand, IZonesCommand zonesCommand)
        {
            _settings = settings;
            _networkHelper = network;
            _azureHelper = azureHelper;
            _serviceHelper = serviceHelper;
            _eventLogger = eventLogger;
            _recordsCommand = recordsCommand;
            _zonesCommand = zonesCommand;

            var result = _serviceHelper.Check();

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            #if DEBUG
            Debugger.Launch();
            #endif

            _eventLogger.LogMessage("Started at " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
            _appTimer = new Timer(_settings.PoolingInterval * 1000);
            _appTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            _appTimer.Start();
        }

        protected override void OnStop()
        {
            _appTimer.Stop();
            _appTimer.Dispose();
            _eventLogger.LogMessage("Obscured.Azure.DynDNS Service was stopped at " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), EventLogEntryType.Warning);
        }

        private static void TimerElapsed(object sender, EventArgs e)
        {
            try
            {
                _appTimer.Stop();
                _eventLogger.LogMessage("Running ip check...");
                var ipAddr = _networkHelper.GetIpAddress(_settings.Provider);
                var result = _serviceHelper.Check();
                _eventLogger.LogMessage("Fetched ip: " + ipAddr);
                _appTimer.Start();
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
            }
        }
    }
}
