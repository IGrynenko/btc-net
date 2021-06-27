namespace BTC.API.Interfaces
{
    public interface ICoinApiRequestSender
    {
        void SendGetRequest(string subPath);
    }
}