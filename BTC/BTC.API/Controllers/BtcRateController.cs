using BTC.API.Interfaces;
using BTC.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BTC.Services.Helpers.Dictionaries;

namespace BTC.API.Controllers
{
    [Route("api/btcRate")]
    [ApiController]
    public class BtcRateController : ControllerBase
    {
        private ICoinApiRequestSender _coinApiRequestSender;
        private readonly Func<string, string, string, string> _subPath = (idBase, idQuote, time) => $"/v1/exchangerate/{idBase}/{idQuote}?time={time}";

        public BtcRateController(ICoinApiRequestSender coinApiRequestSender)
        {
            _coinApiRequestSender = coinApiRequestSender;
        }

        [HttpGet]
        [Authorize]
        public OkResult GetBtcRateInUah()
        {
            var subPath = BuildSubPath(Currency.BTC, Currency.UAH);
            _coinApiRequestSender.SendGetRequest(subPath);

            return Ok();
        }

        private string BuildSubPath(Currency crypro, Currency fiat, string time = null)
        {
            return _subPath.Invoke(Currencies[crypro], Currencies[fiat], time);
        }
    }
}
