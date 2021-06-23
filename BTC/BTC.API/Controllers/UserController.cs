using BTC.API.Models;
using BTC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("test")]
        public OkResult Test()
        {
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<bool> ValidateUser([FromBody] UserModel user)
        {
            //mapper
            var u = _userService.CheckUser(new Services.Models.User() { Name = user.Name, Password = user.Password });
            //generate token
            return Ok(u);
        }
    }
}
