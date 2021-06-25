using BTC.API.Interfaces;
using BTC.API.Models;
using BTC.Services.Interfaces;
using BTC.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost("signup")]
        public void SignupUser([FromBody] UserModel model)
        {

        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> ValidateUser([FromBody] UserModel model)
        {
            try
            {
                var user = await _userService.GetUser(model);

                if (user != null)
                {
                    var token = _tokenService.GenerateJwtToken(user);
                    var response = new UserSuccessResponse
                    {
                        Name = user.Name,
                        Token = token
                    };

                    return Ok(response);
                }
                else
                    return NotFound("User doesn't exist");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
