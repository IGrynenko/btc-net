using BTC.Services;
using BTC.Services.Helpers;
using BTC.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "D:\\Users.csv";
            //CsvHelper<User> aa = new CsvHelper<User>();
            var aa = CsvHelper<User>.Read(path);

            foreach (var item in aa)
            {
                Console.WriteLine(item.Name);
            }
            //var user = new User
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Ivan",
            //    Password = "pass",
            //    Created = "01.01.01"
            //};
            //CsvHelper<User>.WriteTable(path, user);
        }
    }
}
