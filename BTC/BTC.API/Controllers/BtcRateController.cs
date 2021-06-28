using BTC.API.Interfaces;
using BTC.API.Models;
using BTC.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static BTC.Services.Helpers.Dictionaries;

namespace BTC.API.Controllers
{
    [Route("api/btcRate")]
    [ApiController]
    public class BtcRateController : ControllerBase
    {
        private readonly ICoinApiRequestSender _coinApiRequestSender;
        private readonly Func<string, string, string> _subPath = (idBase, idQuote)
            => $"v1/exchangerate/{idBase}/{idQuote}";

        public BtcRateController(ICoinApiRequestSender coinApiRequestSender)
        {
            _coinApiRequestSender = coinApiRequestSender;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<CurrencyInfo>> GetBtcRateInUah()
        {
            var subPath = BuildSubPath(Currency.BTC, Currency.UAH);
            var result = await _coinApiRequestSender.SendGetRequest(subPath);

            if (result == null)
                return StatusCode(500, "Couldn't get response from coinapi.io");

            return Ok(result);
        }

        private string BuildSubPath(Currency crypro, Currency fiat)
        {
            return _subPath.Invoke(Currencies[crypro], Currencies[fiat]);
        }
    }
}
