using BTC.Services.Interfaces;
using BTC.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BTC.Services
{
    public class UserService : IUserService
    {
        private readonly IDataWorker<User> _dataWorker;
        private readonly IMapper _mapper;

        public UserService(IDataWorker<User> dataWorker, IMapper mapper)
        {
            _dataWorker = dataWorker;
            _mapper = mapper;
        }

        public async Task<UserModel> AddUser(UserModel model)
        {
            if (model == null && !string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException();

            var existingUser = await GetUser(model);

            if (existingUser != null)
            {
                //ex?
                return null;
            }

            var user = _mapper.Map<User>(model);
            _dataWorker.WriteTable(user);

            //return name and id
            return model;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.Run(() =>
            {
                IEnumerable<User> allUsers = null;
                try
                {
                    allUsers = _dataWorker.ReadTable();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                return allUsers;
            });
        }

        public async Task<User> GetUser(UserModel model)
        {
            if (model == null && !string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException();

            var users = await GetUsers();

            return users.FirstOrDefault(e => e.Name.Equals(model.Name) && e.Password.Equals(model.Password));
        }
    }
}
