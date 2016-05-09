namespace Obscured.Azure.DynDNS.Core.Models.Records.Type
{
    public class SrvRecord
    {
        public int priority { get; set; }
        public int weight { get; set; }
        public ushort port { get; set; }
        public string target { get; set; }
    }
}
