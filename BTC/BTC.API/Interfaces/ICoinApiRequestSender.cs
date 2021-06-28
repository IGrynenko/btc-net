using BTC.API.Models;
using System.Threading.Tasks;

namespace BTC.API.Interfaces
{
    public interface ICoinApiRequestSender
    {
        Task<CurrencyInfo> SendGetRequest(string subPath);
    }
}