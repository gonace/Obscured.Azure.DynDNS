namespace Obscured.Azure.DynDNS.Core.Models.Records.Type
{
    public class SoaRecord
    {
        public string Email { get; set; }
        public int ExpireTime { get; set; }
        public string Host { get; set; }
        public int MinimumTtl { get; set; }
        public int RefreshTime { get; set; }
        public int RetryTime { get; set; }
        public int SerialNumber { get; set; }
    }
}
