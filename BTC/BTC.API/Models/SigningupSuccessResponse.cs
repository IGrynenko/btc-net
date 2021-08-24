using BTC.Services.Models;

namespace BTC.API.Models
{
    public class SigningupSuccessResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public SigningupSuccessResponse() { }

        public SigningupSuccessResponse(User user)
        {
            Id = user.Id.ToString();
            Name = user.Name;
        }
    }
}
