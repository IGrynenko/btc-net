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
        public async Task<IActionResult> SignupUser([FromBody] UserModel model)
        {
            var user = await _userService.AddUser(model); //add try catch

            if (user == null)
                return BadRequest(new { Message = "User already exists" });

            return Ok(new SigningupSuccessResponse(user));
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
                    var response = new UserValidationSuccessResponse(user, token);

                    return Ok(response);
                }
                else
                    return NotFound("User doesn't exist");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
