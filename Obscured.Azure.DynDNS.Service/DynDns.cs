using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using Obscured.Azure.DynDNS.Core.Helpers;
using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;

namespace Obscured.Azure.DynDNS.Service
{
    public partial class DynDns : ServiceBase
    {
        private static Timer _appTimer;
        private static EventLog _eventLog;
        private static ISettings _settings;
        private static INetwork _networkHelper;
        private static IAzureHelper _azureHelper;

        public DynDns(ISettings settings, INetwork network, IAzureHelper azureHelper)
        {
            _settings = settings;
            _networkHelper = network;
            _azureHelper = azureHelper;

            #region Logging
            _eventLog = new EventLog();
            if (!EventLog.SourceExists("Azure.DynDNS"))
            {
                EventLog.CreateEventSource("Azure.DynDNS", "Obscured");
            }

            _eventLog.Source = "Azure.DynDNS";
            _eventLog.Log = "Obscured";
            #endregion

            /*var jwt = _azureHelper.GetAuthToken(_azureHelper.GetSubscriptionTenantId(_settings.SubscriptionId), _settings.ClientId, _settings.ClientSecret);

            var client = new RestClient(_settings.Azure.BaseUri) { Authenticator = new JwtAuthenticator(jwt) };

            
            var request = new RestRequest(_settings.Azure.RecordUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", "obscured.se");
            request.AddUrlSegment("recordType", "A");
            request.AddUrlSegment("recordSetName", "dyndns");
            var response = client.Execute(request);

            var test = JsonConvert.DeserializeObject<Core.Models.Record>(response.Content);

            var test2 = JsonConvert.SerializeObject(test, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            /*
            var request = new RestRequest(_settings.Azure.ZonesUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            var response = client.Execute(request);
            */

            /*
            var request = new RestRequest(_settings.Azure.RecordsUri, Method.GET);
            request.AddUrlSegment("subscriptionId", _settings.SubscriptionId);
            request.AddUrlSegment("resourceGroupName", _settings.ResourceGroup);
            request.AddUrlSegment("zoneName", "clanwiki.nu");
            var response = client.Execute(request);
            */

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            #if DEBUG
            Debugger.Launch();
            #endif

            _eventLog.WriteEntry("Started at " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
            _appTimer = new Timer(_settings.PoolingInterval * 1000);
            _appTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            _appTimer.Start();

        }

        protected override void OnStop()
        {
            _appTimer.Stop();
            _appTimer.Dispose();
            _eventLog.WriteEntry("Obscured.Azure.DynDNS Service was stopped at " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), EventLogEntryType.Warning);
        }

        private static void TimerElapsed(object sender, EventArgs e)
        {
            _appTimer.Stop();
            try
            {
                _eventLog.WriteEntry("Running ip check...");
                _eventLog.WriteEntry("Fetched ip: " + _networkHelper.GetIpAddress(_settings.Provider));
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
            _appTimer.Start();
        }
    }
}
