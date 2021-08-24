using BTC.API.Interfaces;
using BTC.API.Models;
using BTC.Services.Interfaces;
using BTC.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BTC.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<SigningupSuccessResponse>> SignupUser([FromBody] UserModel model)
        {
            try
            {
                var user = await _userService.AddUser(model);

                if (user == null)
                    return BadRequest(new { Message = "User already exists" });

                return Ok(new SigningupSuccessResponse(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Incorrect user information" });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserValidationSuccessResponse>> ValidateUser([FromBody] UserModel model)
        {
            try
            {
                var user = await _userService.GetUser(model);

                if (user != null)
                {
                    var token = _tokenService.GenerateJwtToken();
                    var response = new UserValidationSuccessResponse(user, token);

                    return Ok(response);
                }
                else
                    return NotFound("User doesn't exist");

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Incorrect user information" });
            }
        }
    }
}
