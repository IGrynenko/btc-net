using BTC.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTC.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUser(UserModel model);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(UserModel model);
    }
}