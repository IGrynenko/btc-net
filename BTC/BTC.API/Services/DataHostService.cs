using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using BTC.Services.Interfaces;
using BTC.Services.Models;
using BTC.API.Interfaces;

namespace BTC.API.Services
{
    public class DataHostService : IDataHostService
    {
        private readonly IDataWorker<User> _userData;

        private string _mainFolder;
        private string _usersTable;

        public DataHostService(IConfiguration configuration, IDataWorker<User> userData)
        {
            _userData = userData;
            _mainFolder = configuration.GetSection("MainFolder").Value;
            _usersTable = configuration.GetSection("DataSource:Users").Value;
        }

        public void StartUp()
        {
            CreateMainFolder();
            CreateUserTable();
        }

        private void CreateMainFolder()
        {
            if (!Directory.Exists(_mainFolder))
                Directory.CreateDirectory(_mainFolder);
        }

        private void CreateUserTable()
        {
            var pathToUsers = $"{_mainFolder}\\{_usersTable}";

            if (!File.Exists(pathToUsers))
                _userData.CreateTable();
        }
    }
}
