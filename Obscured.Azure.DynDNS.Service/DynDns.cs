﻿using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using Newtonsoft.Json;
using Obscured.Azure.DynDNS.Core.Utilities;
using Obscured.Azure.DynDNS.Service.Helpers;

namespace Obscured.Azure.DynDNS.Service
{
    public partial class DynDns : ServiceBase
    {
        private static Timer _appTimer;
        private static ISettings _settings;
        private static IEventLogger _eventLogger;
        private static IServiceHelper _serviceHelper;

        public DynDns(ISettings settings, IServiceHelper serviceHelper, IEventLogger eventLogger)
        {
            _settings = settings;
            _serviceHelper = serviceHelper;
            _eventLogger = eventLogger;

            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void OnStart(string[] args)
        {
            _eventLogger.LogMessage("Started at " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ", with the intervall of " + TimeSpanUtility.ConvertSecondsToMinutes(_settings.PoolingInterval) + "min");
            _appTimer = new Timer(TimeSpanUtility.ConvertSecondsToMilliseconds(_settings.PoolingInterval));
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
                var result = _serviceHelper.Check();

                if (result.Success)
                {
                    _eventLogger.LogMessage(result.Updated
                        ? $"IP Address was updated from {result.OldAddress} to {result.NewAddress}"
                        : $"IP Address was not updated since the old ({result.OldAddress}) and new ({result.NewAddress}) are the same");
                }
                else
                {
                    _eventLogger.LogMessage(result.Exception.Message, EventLogEntryType.Error);
                }
                _appTimer.Start();
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(JsonConvert.SerializeObject(ex), EventLogEntryType.Error);
            }
        }

        #region Exception Handling
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }
        #endregion
    }
}
