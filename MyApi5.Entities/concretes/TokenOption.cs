using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Entities.concretes
{
    public class TokenOption
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int AccessTokenExpirationTime { get; set; }
        public string? SecurityKey { get; set; }
    }
}
