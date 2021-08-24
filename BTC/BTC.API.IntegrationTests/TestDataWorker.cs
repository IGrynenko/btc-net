using BTC.Services.Interfaces;
using BTC.Services.Models;
using System;
using System.Collections.Generic;

namespace BTC.API.IntegrationTests
{
    public class TestDataWorker<T> : IDataWorker<T>
        where T : class
    {
        public List<User> testUsers { get; private set; } = new List<User>
            {
                new User { Id = new Guid("2a955059-537f-4206-8267-0e9188a29379"), Name = "Test_1", Password = "Pass_1", Created = "2021/1/1" },
                new User { Id = new Guid("5c51f2c3-f3c3-45f7-93f3-95e8674430d5"), Name = "Test_2", Password = "Pass_2", Created = "2021/1/1" },
                new User { Id = new Guid("e8ce1d6a-f2ba-4489-98c9-4b66137cb241"), Name = "Test_3", Password = "Pass_3", Created = "2021/1/1" },
                new User { Id = new Guid("dbc873d4-1adb-435a-a8c9-e0a9133fd1df"), Name = "Test_4", Password = "Pass_4", Created = "2021/1/1" },
                new User { Id = new Guid("871d1a9a-b9f9-404f-8a28-a68a4b923924"), Name = "Test_5", Password = "Pass_5", Created = "2021/1/1" },
            };

        public void CreateTable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ReadTable()
        {
            return (IEnumerable<T>)testUsers;
        }

        public void WriteTable(T row)
        {
            testUsers.Add(row as User);
        }
    }
}
