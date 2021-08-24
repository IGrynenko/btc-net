using BTC.API.Helpers;
using BTC.API.Interfaces;
using BTC.API.Models;
using FakeItEasy;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BTC.API.Services.Tests
{
    public class CoinApiRequestSenderTests
    {
        private ICoinApiRequestSender _coinApiRequestSender;
        private IRestClient _client;

        public CoinApiRequestSenderTests()
        {
            _client = A.Fake<IRestClient>();
            var coinApiSettings = A.Fake<IOptions<CoinApiSettings>>();
            A.CallTo(() => coinApiSettings.Value).Returns(new CoinApiSettings
            {
                Key = "123",
                Path = "https://rest.coinapi.io/"
            });
            _coinApiRequestSender = new CoinApiRequestSender(coinApiSettings, _client);
        }

        [Theory]
        [InlineData(22.1)]
        public async Task SendGetRequest_WhenSuccesfulResponse_ThenReturnCurrencyInfo(decimal rate)
        {
            var request = new RestRequest(Method.GET);
            var token = new CancellationToken();
            A.CallTo(_client).Where(call => call.Method.Name == "ExecuteAsync")
                .WithReturnType<Task<IRestResponse>>()
                .Returns(new RestResponse
                {
                    Content = string.Format("{{rate: {0}}}", rate),
                    StatusCode = HttpStatusCode.OK
                });

            var result = await _coinApiRequestSender.SendGetRequest(null);

            Assert.NotNull(result);
            Assert.True(typeof(CurrencyInfo) == result.GetType());
            Assert.True(rate == result.Rate);
        }

        [Fact]
        public async Task SendGetRequest_WhenUnsuccesfulResponse_ThenReturnNull()
        {
            var request = new RestRequest(Method.GET);
            var token = new CancellationToken();
            A.CallTo(_client).Where(call => call.Method.Name == "ExecuteAsync")
                .WithReturnType<Task<IRestResponse>>()
                .Returns(new RestResponse
                {
                    Content = string.Empty,
                    StatusCode = HttpStatusCode.BadRequest
                });

            var result = await _coinApiRequestSender.SendGetRequest(null);

            Assert.Null(result);
        }
    }
}
