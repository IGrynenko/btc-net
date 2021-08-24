using BTC.Services.Models;

namespace BTC.API.Models
{
    public class UserValidationSuccessResponse : SigningupSuccessResponse
    {
        public string Token { get; set; }

        public UserValidationSuccessResponse() { }

        public UserValidationSuccessResponse(User user) : base(user) { }

        public UserValidationSuccessResponse(User user, string token) : this(user)
        {
            Token = token;
        }
    }
}
