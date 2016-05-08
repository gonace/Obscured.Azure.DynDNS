namespace Obscured.Azure.DynDNS.Core.Models.Records.Type
{
    public class SrvRecord
    {
        public int Priority { get; set; }
        public int Weight { get; set; }
        public ushort Port { get; set; }
        public string Target { get; set; }
    }
}
