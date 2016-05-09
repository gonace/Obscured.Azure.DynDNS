using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
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

        public Core.Models.Result Check()
        {
            var result = new Core.Models.Result();

            try
            {
                IPAddress ipAddress;
                var ipAddresses = _networkHelper.GetIpAddress(_settings.Providers);
                result.Providers = ipAddresses;
                _eventLogger.LogMessage(JsonConvert.SerializeObject(ipAddresses), EventLogEntryType.Information);
                if (ipAddresses.Count > 1)
                {
                    ipAddress = IPAddress.Parse(ipAddresses.GroupBy(x => x.ReturnedAddress).OrderByDescending(ip => ip.Count()).First().First().ReturnedAddress);
                    _eventLogger.LogMessage(String.Format("Used the most common response, which was: {0}", ipAddress), EventLogEntryType.Information);
                }
                else if (ipAddresses.Count == 1)
                {
                    ipAddress = IPAddress.Parse(ipAddresses.First().ReturnedAddress);
                    _eventLogger.LogMessage(String.Format("Used response from provider: {0}, which was: {1}", ipAddresses.First().Name, ipAddress), EventLogEntryType.Information);
                }
                else
                {
                    throw new NullReferenceException("No ip-addresses could be fetch by selected providers");
                }
                result.NewAddress = ipAddress;


                var record = _recordsCommand.Get(_settings.RecordName, _settings.RecordType);
                _eventLogger.LogMessage("External ip-address was concluded to be: " + ipAddress);

                if (record == null || record.properties == null)
                {
                    throw new NullReferenceException(String.Format("No record could be fetch from Azure, make sure the record that should be updates exsist, record: {0}", (_settings.RecordName + '.' + _settings.ZoneName)));
                }

                if (record.properties.ARecords.Count > 0)
                {
                    var oldAddress = record.properties.ARecords.FirstOrDefault();
                    if (oldAddress != null)
                        result.OldAddress = IPAddress.Parse(oldAddress.ipv4Address);
                }

                if (record.properties.ARecords.Any(x => !x.ipv4Address.Equals(ipAddress.ToString())))
                {
                    result.Updated = Update(record, ipAddress);
                }
                result.Success = true;

                return result;
            }
            catch (Exception ex)
            {
                _eventLogger.LogMessage(ex.Message, EventLogEntryType.Error);
                result.Exception = ex;
                return result;
            }
        }

        private static bool Update(Core.Models.Record record, IPAddress newIp)
        {
            try
            {
                record.properties.ARecords.Clear();
                record.properties.ARecords.Insert(0, new ARecord { ipv4Address = newIp.ToString() });

                var resp = _recordsCommand.Update(record, _settings.ZoneName);

                if (resp.GetType().FullName == "Obscured.Azure.DynDNS.Core.Models.Record")
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
