using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Obscured.Azure.DynDNS.Core.Commands;
using Obscured.Azure.DynDNS.Core.Models.Records.Type;
using Obscured.Azure.DynDNS.Core.Utilities;

namespace Obscured.Azure.DynDNS.Service.Helpers
{
    public class ServiceHelper : IServiceHelper
    {
        private static ISettings _settings;
        private static IEventLogger _eventLogger;
        private static INetwork _networkHelper;
        private static IRecordsCommand _recordsCommand;
        private static IZonesCommand _zonesCommand;

        public ServiceHelper(ISettings settings, IEventLogger eventLogger, INetwork network, IRecordsCommand recordsCommand, IZonesCommand zonesCommand)
        {
            _settings = settings;
            _eventLogger = eventLogger;
            _networkHelper = network;
            _recordsCommand = recordsCommand;
            _zonesCommand = zonesCommand;
        }

        public bool Check()
        {
            var ip = _networkHelper.GetIpAddress();
            var record = _recordsCommand.Get(_settings.RecordName, "A");

            if (record == null)
            {
                throw new NullReferenceException();
            }

            if (record.Properties.ARecords.Any(x => !x.Ipv4Address.Equals(ip.ToString())))
            {
                _eventLogger.LogMessage(String.Format("New ip adress found {0}", ip), EventLogEntryType.Information);
                return Update(ip);
            }

            return true;
        }

        private static bool Update(IPAddress newIp)
        {
            try
            {
                var record = _recordsCommand.Get(_settings.RecordName, "A");
                record.Properties.ARecords.Clear();
                record.Properties.ARecords.Insert(0, new ARecord { Ipv4Address = newIp.ToString()});

                return true;
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
            }

            return false;
        }
    }
}
