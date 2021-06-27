using BTC.API.Helpers;
using BTC.API.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTC.API.Services
{
    public class CoinApiRequestSender : ICoinApiRequestSender
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<CoinApiSettings> _coinApiSettings;

        public CoinApiRequestSender(IHttpClientFactory clientFactory, IOptions<CoinApiSettings> coinApiSettings)
        {
            _clientFactory = clientFactory;
            _coinApiSettings = coinApiSettings;
        }

        public void SendGetRequest(string subPath)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_coinApiSettings.Value.Path + subPath)
            };
            request.Headers.Add("X-CoinAPI-Key", _coinApiSettings.Value.Key);

            var response = client.SendAsync(request);

            if (response.IsCompletedSuccessfully)
            {
                var result = response.Result;
            }
        }
    }
}
