using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.API.Controllers
{
    [Route("api/btcRate")]
    [ApiController]
    public class BtcRateController : ControllerBase
    {
        [HttpGet("test")]
        [Authorize]
        public OkResult Test()
        {
            return Ok();
        }
    }
}
