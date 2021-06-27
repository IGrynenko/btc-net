using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.API.Models
{
    public class UserValidationSuccessResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
