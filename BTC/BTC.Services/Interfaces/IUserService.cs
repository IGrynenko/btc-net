using BTC.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTC.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> AddUser(UserModel model);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(UserModel model);
    }
}