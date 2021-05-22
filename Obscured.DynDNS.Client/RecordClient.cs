using System.Threading.Tasks;

namespace Obscured.DynDNS.Client
{
    public interface IRecordClient
    {
        Task Update();
    }

    public class RecordCommand : BaseClient, IRecordClient
    {
        public RecordCommand(IOptions options)
            : base(options)
        {
        }

        public async Task Update()
        {
            DnsZone.Update().DefineARecordSet("");
        }
    }
}
