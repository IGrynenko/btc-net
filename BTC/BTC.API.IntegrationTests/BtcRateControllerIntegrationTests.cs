using BTC.API.Models;
using BTC.Services.Interfaces;
using BTC.Services.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BTC.API.IntegrationTests
{
    public class BtcRateControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public BtcRateControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddSingleton<IDataWorker<User>, TestDataWorker<User>>());
            })
            .CreateClient();
        }

        [Theory]
        [InlineData("Test_1", "Pass_1")]
        public async Task GetBtcRateInUah_WhenSuccesfulRequest_ThenReturnCurrencyInfo(string name, string password)
        {
            var model = new UserModel { Name = name, Password = password };
            var userJson = JsonConvert.SerializeObject(model);
            CurrencyInfo btcResponse = null;

            var validtionResult = await _client.PostAsync("api/user/login", new StringContent(userJson, Encoding.UTF8, "application/json"));

            if (validtionResult.StatusCode == HttpStatusCode.OK)
            {
                var validationResponse = JsonConvert.DeserializeObject<UserValidationSuccessResponse>(await validtionResult.Content.ReadAsStringAsync());
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {validationResponse?.Token}");
                var btcResult = await _client.GetAsync("api/btcRate");
                btcResponse = JsonConvert.DeserializeObject<CurrencyInfo>(await btcResult.Content.ReadAsStringAsync());
            }

            Assert.NotNull(btcResponse);
            Assert.NotEqual(0, btcResponse.Rate);
        }

        [Fact]
        public async Task GetBtcRateInUah_WhenUnauthorizedRequest_ThenReturnBadRequest()
        {
            var result = await _client.GetAsync("api/btcRate");

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
