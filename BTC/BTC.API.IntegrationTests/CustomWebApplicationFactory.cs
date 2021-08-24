using Microsoft.AspNetCore.Mvc.Testing;

namespace BTC.API.IntegrationTests
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T>
        where T : class
    { }
}
