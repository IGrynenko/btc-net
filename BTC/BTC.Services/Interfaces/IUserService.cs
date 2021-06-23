using BTC.Services.Models;

namespace BTC.Services.Interfaces
{
    public interface IUserService
    {
        void AddUser();
        bool CheckUser(User user);
    }
}