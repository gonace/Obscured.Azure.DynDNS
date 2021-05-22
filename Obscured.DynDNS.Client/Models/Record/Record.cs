using System.Collections.Generic;
using Newtonsoft.Json;
using Obscured.DynDNS.Client.Models.Record.Type;

namespace Obscured.DynDNS.Client.Models.Record
{
    public class Properties
    {
        [JsonProperty("Metadata")]
        public string Metadata { get; set; }
        [JsonProperty("fqdn")]
        public string Fqdn { get; set; }
        [JsonProperty("ttl")]
        public int Ttl { get; set; }

        public IList<ARecord> ARecords { get; set; }
        public IList<AaaaRecord> AAAARecords { get; set; }
        public CnameRecord CNAMERecords { get; set; }
        public IList<MxRecord> MXRecords { get; set; }
        public IList<NsRecord> NSRecords { get; set; }
        public SoaRecord SSOARecord { get; set; }
        public IList<SrvRecord> SRVRecord { get; set; }
        public IList<TxtRecords> TXTRecords { get; set; }
    }
}
