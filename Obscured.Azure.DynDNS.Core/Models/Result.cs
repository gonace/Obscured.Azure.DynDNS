using System;
using System.Collections.Generic;
using System.Net;

namespace Obscured.Azure.DynDNS.Core.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public bool Updated { get; set; }
        public Exception Exception { get; set; }

        public IPAddress OldAddress { get; set; }
        public IPAddress NewAddress { get; set; }
        public IList<Provider> Providers { get; set; }

        public Result()
        {
            Success = false;
            Updated = false;
        }
    }
}
