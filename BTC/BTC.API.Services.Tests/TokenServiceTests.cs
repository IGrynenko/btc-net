using BTC.API.Helpers;
using BTC.API.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace BTC.API.Services.Tests
{
    public class TokenServiceTests
    {
        private ITokenService _tokenService;
        private IOptions<JwtTokenSettings> _jwtTokenSettings;

        [Fact]
        public void GenerateJwtToken_WhenKeyIsSet_ThenGenetateToken()
        {
            _jwtTokenSettings = A.Fake<IOptions<JwtTokenSettings>>();
            A.CallTo(() => _jwtTokenSettings.Value).Returns(new JwtTokenSettings
            {
                Audiance = "audiance",
                Issuer = "issuer",
                Key = "some_key_some_key_some_key_some_key",
                LifeSpan = "1"
            });
            _tokenService = new TokenService(_jwtTokenSettings);

            var token = _tokenService.GenerateJwtToken();

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateJwtToken_WhenKeyIsNotSet_ThenThrowException()
        {
            _jwtTokenSettings = A.Fake<IOptions<JwtTokenSettings>>();
            A.CallTo(() => _jwtTokenSettings.Value).Returns(new JwtTokenSettings
            {
                Audiance = "audiance",
                Issuer = "issuer",
                Key = "",
                LifeSpan = "1"
            });
            _tokenService = new TokenService(_jwtTokenSettings);

            Assert.Throws<ArgumentException>(() => _tokenService.GenerateJwtToken());
        }
    }
}
