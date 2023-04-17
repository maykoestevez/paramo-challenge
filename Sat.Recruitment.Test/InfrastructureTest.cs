using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure;
using Sat.Recruitment.Infrastructure.Data;
using Sat.Recruitment.Infrastructure.Files;
using Sat.Recruitment.Infrastructure.Helpers;
using Sat.Recruitment.Infrastructure.Interfaces;
using Sat.Recruitment.Infrastructure.Repository;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class InfrastructureTest
    {
        private readonly UserRepository _userRepository;

        public InfrastructureTest()
        {
            var userDataReader = new Mock<IUserDataReader>();
            var users = new List<User>()
            {
                new User("Pedro", "something@gmail.com", "Some where", "80945588")
            };
            userDataReader.Setup(x => x.GetData()).Returns(users);
            _userRepository = new UserRepository(userDataReader.Object);
        }


        [Fact]
        public void EmailShouldBeValid()
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => Helper.NormalizeEmail("email.com"));
            Assert.True(ex.Message == Messages.InvalidEmail);
        }

        [Fact]
        public void InvalidCharactersShouldBeRemovedFromEmail()
        {
            //Act
            var normalizedEmail = Helper.NormalizeEmail("e+mail@gmail.com");

            //Assert
            const string expected = "email@gmail.com";
            Assert.True(expected == normalizedEmail);
        }


        [Fact]
        public async Task WeShouldBeAbleToAddUserFromRepository()
        {
            //Arrange
            var userToInsert = new User("test1", "email@hotmail.com", "Somewhere", "809345445")
            {
                Money = 100,
                UserType = UserType.Normal
            };

            //Act
            var result = await _userRepository.Add(userToInsert);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Name == userToInsert.Name);
        }

        [Fact]
        public async Task UserShouldNotBeNull()
        {
            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userRepository.Add(null));
            Assert.True(ex.Message.Length > 0);
            Assert.True(ex.Message == Messages.UserShouldNotBeNull);
        }

        [Fact]
        public async Task UserShouldNotBeDuplicated()
        {
            //Arrange
            var userToInsert = new User("Pedro", "something@gmail.com", "Some where", "80945588");

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userRepository.Add(userToInsert));
            Assert.True(ex.Message.Length > 0);
            Assert.True(ex.Message == Messages.DuplicatedUser + userToInsert.Name);
        }

        [Fact]
        public void UserTXTFileShouldBeRead()
        {
            //Arrange
            var fileReader = new FileReader();
            var stream = fileReader.ReadFile("/users.txt");

            //Act
            var character = stream.Peek();
            stream.Close();

            //Assert
            Assert.NotNull(stream);
            Assert.True(character >= 0);
        }

        [Fact]
        public void DataToReadThrowFileNotFoundException()
        {
            //Act
            var fileReader = new FileReader();

            //Assert
            var ex = Assert.Throws<FileNotFoundException>(() => fileReader.ReadFile("/data.txt"));
            Assert.True(ex.Message.Length > 0);
        }

        [Fact]
        public async Task UserDataReaderShouldReadUserDataFromFileReader()
        {
            //Arrange
            var fileReader = new FileReader();
            var inMemorySettings = new Dictionary<string, string>
            {
                {"UserFile", "/users.txt"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var userDataReader = new UserDataReader(fileReader, configuration);

            //Act
            await userDataReader.ReadData();
            var users = userDataReader.GetData();

            //Assert
            Assert.NotEmpty(users);
        }
    }
}