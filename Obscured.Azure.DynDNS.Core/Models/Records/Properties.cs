using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models.Records
{
    public class Properties
    {
        public string metadata { get; set; }
        public string fqdn { get; set; }
        public int TTL { get; set; }

        public IList<Type.ARecord> ARecords { get; set; }
        public IList<Type.AaaaRecord> AAAARecords { get; set; }
        public Type.CnameRecord CNAMERecords { get; set; }
        public IList<Type.MxRecord> MXRecords { get; set; }
        public IList<Type.NsRecord> NSRecords { get; set; }
        public Type.SoaRecord SSOARecord { get; set; }
        public IList<Type.SrvRecord> SRVRecord { get; set; }
        public IList<Type.TxtRecords> TXTRecords { get; set; }
    }
}
