using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class ObscuredJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public ObscuredJsonSerializer(JsonSerializerSettings settings)
        {
            ContentType = "application/json";
		    _settings = settings;
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }


	    public string Serialize(object obj) {
		    return JsonConvert.SerializeObject(obj, _settings);
	    }

        public T Deserialize<T>(string s) {
		    return JsonConvert.DeserializeObject<T>(s, _settings);
	    }
    }
}
