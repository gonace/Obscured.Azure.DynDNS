using System.Collections.Generic;

namespace Obscured.Azure.DynDNS.Core.Models.Records
{
    public class Properties
    {
        public string Metadata { get; set; }
        public string Fqdn { get; set; }
        public string Ttl { get; set; }

        public IList<Type.ARecord> ARecords { get; set; }
        public Type.CnameRecord CnameRecords { get; set; }
        public IList<Type.MxRecord> MxRecords { get; set; }
        public IList<Type.NsRecord> NsRecords { get; set; }
        public Type.SoaRecord SoaRecord { get; set; }
        public IList<Type.SrvRecord> SrvRecord { get; set; }
        public IList<Type.TxtRecords> TxtRecords { get; set; }
    }
}
