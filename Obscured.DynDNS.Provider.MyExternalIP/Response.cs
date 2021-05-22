﻿using Newtonsoft.Json;

namespace Obscured.DynDNS.Provider.MyExternalIP
{
    /// <summary>
    /// {
    ///   "ip": "217.215.208.73"
    /// }
    /// </summary>
    public class Response
    {
        [JsonProperty("ip")]
        public string Address { get; set; }
    }
}