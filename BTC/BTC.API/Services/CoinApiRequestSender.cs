using BTC.API.Helpers;
using BTC.API.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using BTC.API.Models;

namespace BTC.API.Services
{
    public class CoinApiRequestSender : ICoinApiRequestSender, IDisposable
    {
        private readonly IOptions<CoinApiSettings> _coinApiSettings;
        private RestClient _client;

        public CoinApiRequestSender(IOptions<CoinApiSettings> coinApiSettings)
        {
            _coinApiSettings = coinApiSettings;
            _client = new RestClient();
            _client.BaseUrl = new Uri(coinApiSettings.Value.Path);
        }

        public async Task<CurrencyInfo> SendGetRequest(string subPath = null)
        {
            var request = new RestRequest(Method.GET);

            if (!string.IsNullOrEmpty(subPath))
                _client.BaseUrl = new Uri(_coinApiSettings.Value.Path + subPath);

            request.AddHeader("X-CoinAPI-Key", _coinApiSettings.Value.Key);
            var response = await _client.ExecuteAsync(request);

            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var currencyInfo = JsonConvert.DeserializeObject<CurrencyInfo>(response.Content);
                    return currencyInfo;
                }
                return null;
            }
            return null;

        }

        public void Dispose()
        {
            _client = null;
        }
    }
}
