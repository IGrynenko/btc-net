using BTC.API.Helpers;
using BTC.API.Interfaces;
using BTC.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BTC.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly double defaultTime = 100d;

        IConfiguration _configuration;

        public TokenService(IOptions<JwtTokenSettings> options, IConfiguration configuration)
        {
            _jwtTokenSettings = options.Value;
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var timeNow = DateTime.Now;
            var timeSpan = double.TryParse(_jwtTokenSettings.LifeSpan, out double time) ? time : defaultTime;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtTokenSettings.Key);
            var descriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                NotBefore = timeNow,
                Expires = timeNow.AddMinutes(timeSpan),
                Audience = _jwtTokenSettings.Audiance,
                Issuer = _jwtTokenSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
