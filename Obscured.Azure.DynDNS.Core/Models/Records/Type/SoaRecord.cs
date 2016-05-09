namespace Obscured.Azure.DynDNS.Core.Models.Records.Type
{
    public class SoaRecord
    {
        public string email { get; set; }
        public int expireTime { get; set; }
        public string host { get; set; }
        public int minimumTTL { get; set; }
        public int refreshTime { get; set; }
        public int retryTime { get; set; }
        public int serialNumber { get; set; }
    }
}
