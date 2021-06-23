using BTC.Services.Interfaces;
using BTC.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BTC.Services
{
    public class UserService : IUserService
    {
        private readonly IDataWorker<User> _dataWorker;

        public UserService(IDataWorker<User> dataWorker)
        {
            _dataWorker = dataWorker;
        }

        public void AddUser()
        {

        }

        public bool CheckUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            //async
            var allUsers = _dataWorker.ReadTable();
            var exactUser = allUsers.FirstOrDefault(e => e.Name.Equals(user.Name) && e.Password.Equals(user.Password));

            return exactUser != null;
        }
    }
}
