using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure.Interfaces;

namespace Sat.Recruitment.Infrastructure.Data
{
    /// <summary>
    /// Manage user data reader
    /// </summary>
    public class UserDataReader : IUserDataReader
    {
        private readonly IFileReader _fileReader;
        private readonly string _filePath;
        private readonly IList<User> _users = new List<User>();

        public UserDataReader(IFileReader fileReader, IConfiguration configuration)
        {
            _fileReader = fileReader;
            _filePath = configuration["UserFile"] ?? "";
             ReadData().Wait();
        }

        public async Task ReadData()
        {
            var reader = _fileReader.ReadFile(_filePath);
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();

                var name = line.Split(',')[0];
                var email = line.Split(',')[1];
                var address = line.Split(',')[2];
                var phone = line.Split(',')[3];
                var newUser = new User(name, email, address, phone)
                {
                    UserType = Enum.Parse<UserType>(line.Split(',')[4]),
                    Money = decimal.Parse(line.Split(',')[5]),
                };

                _users.Add(newUser);
            }

            reader.Close();
        }

        public IEnumerable<User> GetData() => _users;
    }
}