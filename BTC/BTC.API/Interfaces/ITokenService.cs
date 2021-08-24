using BTC.Services.Models;

namespace BTC.API.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken();
    }
}