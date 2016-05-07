using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using Obscured.Azure.DynDNS.Core.Utilities;

using Microsoft.Azure;
using Microsoft.Azure.Management.Dns;
using Microsoft.Azure.Management.Dns.Models;

namespace Obscured.Azure.DynDNS.Service
{
    public partial class DynDns : ServiceBase
    {
        private static Timer _appTimer;
        private static EventLog _eventLog;
        private static ISettings _settings;
        private static INetwork _networkHelper;

        public DynDns(ISettings settings, INetwork network)
        {
            _settings = settings;
            _networkHelper = network;
            AutoLog = false;

            var subID = _settings.SubscriptionId;
            var ip = _networkHelper.GetIpAddress();
            /*
            if (!EventLog.SourceExists("Azure.DynDNS"))
                EventLog.CreateEventSource("Azure.DynDNS", "Obscured");

            _eventLog.Source = "Azure.DynDNS";
            _eventLog.Log = "Obscured";
            */

            //string jwt = GetAToken();
            //var tcCreds = new TokenCloudCredentials(subID, jwt);

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            #if DEBUG
            Debugger.Launch();
            #endif

            _appTimer = new Timer(60 * 1000);
            _appTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            _appTimer.Start();

        }

        protected override void OnStop()
        {
            _eventLog.WriteEntry("Obscured.Azure.DynDNS Service was stopped.", EventLogEntryType.Warning);
        }

        private static void TimerElapsed(object sender, EventArgs e)
        {
            _appTimer.Stop();
            _eventLog.WriteEntry("Started at" + DateTime.Now);
            try
            {
                _eventLog.WriteEntry("Checking if ip address was changed since last run");

            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
            _appTimer.Start();
        }
    }
}
