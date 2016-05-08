using Obscured.Azure.DynDNS.Core.Utilities;
using RestSharp;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public class BaseCommand : IBaseCommand
    {
        private RestClient _restClient;
        private Settings _settings;

        public BaseCommand(RestClient restClient, Settings settings)
        {
            _settings = settings;
            _restClient = restClient;
        }
    }
}
