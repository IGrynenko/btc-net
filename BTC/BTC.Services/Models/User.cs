using System;

namespace BTC.Services.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Created { get; set; }
    }
}
