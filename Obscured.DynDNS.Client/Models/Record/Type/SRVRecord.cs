namespace Obscured.DynDNS.Client.Models.Record.Type
{
    public class SrvRecord
    {
        public int priority { get; set; }
        public int weight { get; set; }
        public ushort port { get; set; }
        public string target { get; set; }
    }
}
