using AutoMapper;
using BTC.Services.Interfaces;
using BTC.Services.Models;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BTC.Services.Tests
{
    public class UserServiceTests
    {
        private IDataWorker<User> _dataWorker;
        private IMapper _mapper;
        private IUserService _userService;
        private List<User> _users;
        private User _specificUser;

        public UserServiceTests()
        {
            _users = new List<User>();

            for (int i = 0; i < 19; i++)
            {
                _users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Name = $"Test user {i + 1}",
                    Password = $"Pass {i}",
                    Created = DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo)
                });
            }

            _specificUser = new User
            {
                Id = new Guid("d06c0cca-cc80-4282-aa28-50702bf4e74e"),
                Password = "Password123",
                Name = "Specific user",
                Created = DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo)
            };
            _users.Add(_specificUser);
            _dataWorker = A.Fake<IDataWorker<User>>();
            _mapper = A.Fake<IMapper>();
            _userService = new UserService(_dataWorker, _mapper);
        }

        [Fact]
        public async Task GetUsers_WhenTableExists_ThenReturnUsers()
        {
            A.CallTo(() => _dataWorker.ReadTable()).Returns(_users);

            var users = await _userService.GetUsers();

            Assert.NotEmpty(users);
            Assert.Equal(users.Count(), _users.Count());
        }

        [Fact]
        public async Task GetUsers_WhenTableDoesntExist_ThenThrowsExeption()
        {
            A.CallTo(() => _dataWorker.ReadTable()).Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => _userService.GetUsers());
        }

        [Fact]
        public async Task GetUser_WhenNameAndPasswordExist_ThenReturnUser()
        {
            A.CallTo(() => _dataWorker.ReadTable()).Returns(_users);
            var userToFind = new UserModel { Name = _specificUser.Name, Password = _specificUser.Password };

            var user = await _userService.GetUser(userToFind);

            Assert.True(user.Id == _specificUser.Id);
        }

        [Theory]
        [InlineData("Test user 1", "Invalid password")]
        [InlineData("Invalid user name", "Pass 1")]
        public async Task GetUser_WhenNameOrPasswordDontExist_ThenReturnNull(string name, string password)
        {
            A.CallTo(() => _dataWorker.ReadTable()).Returns(_users);
            var userToFind = new UserModel { Name = name, Password = password};

            var user = await _userService.GetUser(userToFind);

            Assert.Null(user);
        }

        [Fact]
        public async Task GetUser_WhenUserIsNull_ThenThrowException()
        {
            UserModel userToFind = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUser(userToFind));
        }

        [Theory]
        [InlineData("Test user 1", null)]
        [InlineData(null, "Pass 1")]
        public async Task GetUser_WhenUserOrPasswordIsNull_ThenThrowException(string name, string password)
        {
            var userToFind = new UserModel { Name = name, Password = password };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUser(userToFind));
        }

        [Fact]
        public async Task AddUser_WhenNameAndPassExist_ThenWriteTableAndReturnUser()
        {
            var newUser = new UserModel { Name = "New user", Password = "Some pass" };
            var mappedUser = new User { Name = "New user" };
            A.CallTo(() => _dataWorker.ReadTable()).Returns(_users);
            A.CallTo(() => _mapper.Map<User>(newUser)).Returns(mappedUser);

            var user = await _userService.AddUser(newUser);

            Assert.NotNull(user);
            Assert.True(user.Name == newUser.Name);
            A.CallTo(() => _dataWorker.WriteTable(mappedUser)).MustHaveHappened();
        }

        [Fact]
        public async Task AddUser_WhenSuchUserExists_ThenReturnNull()
        {
            A.CallTo(() => _dataWorker.ReadTable()).Returns(_users);
            var newUser = new UserModel { Name = _specificUser.Name, Password = _specificUser.Password };

            var user = await _userService.AddUser(newUser);

            Assert.Null(user);
        }

        [Fact]
        public async Task AddUser_WhenUserIsNull_ThenThrowException()
        {
            UserModel newUser = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.AddUser(newUser));
        }

        [Theory]
        [InlineData("Test user 1", null)]
        [InlineData(null, "Pass 1")]
        public async Task AddUser_WhenUserOrPasswordIsNull_ThenThrowException(string name, string password)
        {
            var newUser = new UserModel { Name = name, Password = password };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.AddUser(newUser));
        }
    }
}
