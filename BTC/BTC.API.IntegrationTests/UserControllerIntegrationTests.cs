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
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddSingleton<IDataWorker<User>, TestDataWorker<User>>());
            })
            .CreateClient();
        }

        [Theory]
        [InlineData("Test_1", "")]
        [InlineData("", "Pass_2")]
        [InlineData("Test_2", null)]
        public async Task SignupUser_WhenInvalidModel_ThenReturnBadRequest(string name, string password)
        {
            var model = new UserModel { Name = name, Password = password };
            var userJson = JsonConvert.SerializeObject(model);

            var result = await _client.PostAsync("api/user/create", new StringContent(userJson, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Theory]
        [InlineData("Test_1", "Pass_1")]
        [InlineData("Test_2", "Pass_2")]
        public async Task SignupUser_WhenUserExists_ThenReturnBadRequest(string name, string password)
        {
            var model = new UserModel { Name = name, Password = password };
            var userJson = JsonConvert.SerializeObject(model);

            var result = await _client.PostAsync("api/user/create", new StringContent(userJson, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task SignupUser_WhenValidNewUser_ThenReturnOk()
        {
            var model = new UserModel { Name = "New_user", Password = "New_password" };
            var userJson = JsonConvert.SerializeObject(model);

            var result = await _client.PostAsync("api/user/create", new StringContent(userJson, Encoding.UTF8, "application/json"));
            var response = JsonConvert.DeserializeObject<SigningupSuccessResponse>(await result.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Id);
            Assert.NotNull(response.Name);
        }

        [Theory]
        [InlineData("Test_1", "Pass_1")]
        [InlineData("Test_2", "Pass_2")]
        public async Task ValidateUser_WhenValidExistingUser_ThenReturnOkAndData(string name, string password)
        {
            var model = new UserModel { Name = name, Password = password };
            var userJson = JsonConvert.SerializeObject(model);

            var result = await _client.PostAsync("api/user/login", new StringContent(userJson, Encoding.UTF8, "application/json"));
            var response = JsonConvert.DeserializeObject<UserValidationSuccessResponse>(await result.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(response);
            Assert.NotEmpty(response.Id);
            Assert.NotEmpty(response.Name);
            Assert.NotEmpty(response.Token);
        }

        [Theory]
        [InlineData("Test_1", "")]
        [InlineData("", "Pass_2")]
        [InlineData("Test_2", null)]
        public async Task ValidateUser_WhenInvalidModel_ThenReturnBadRequest(string name, string password)
        {
            var model = new UserModel { Name = name, Password = password };
            var userJson = JsonConvert.SerializeObject(model);

            var result = await _client.PostAsync("api/user/login", new StringContent(userJson, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Theory]
        [InlineData("Test_111", "Pass_111")]
        [InlineData("Test_222", "Pass_222")]
        public async Task ValidateUser_WhenNotExistingUser_ThenReturnNotFound(string name, string password)
        {
            var model = new UserModel { Name = name, Password = password };
            var userJson = JsonConvert.SerializeObject(model);

            var result = await _client.PostAsync("api/user/login", new StringContent(userJson, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
