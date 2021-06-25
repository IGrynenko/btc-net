using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.API.Helpers
{
    public class JwtTokenSettings
    {
        public string Issuer { get; set; }
        public string Audiance { get; set; }
        public string Key { get; set; }
        public string LifeSpan { get; set; }
    }
}
