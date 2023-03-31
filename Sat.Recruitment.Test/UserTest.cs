using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserTest
    {
        private readonly UserService _userService;
        private readonly IFileService _fileService;

        public UserTest()
        {
            var fileService = new Mock<IFileService>();
            fileService.Setup(x => x.ReadUsersFromFile()).Returns(getStreamReader());
            _userService = new UserService(fileService.Object);
        }

        [Fact]
        public async Task UserShouldBeCreatedSuccessfully()
        {
            //Arrange
            var userToInsert = new User()
            {
                Name = "Mayko Estevez",
                Phone = "8094554444",
                Email = "fakeemail@gmail.com",
                Address = "Some where out there",
                Money = 105,
                UserType = UserType.Normal
            };

            //Act
            var userResult = await _userService.CreateUser(userToInsert);

            //Assert
            Assert.Equal(true, userResult.IsSuccessful);
            Assert.NotNull(userResult.DataResult);
        }
        
        [Theory]
        [InlineData(null, "8093721840", "Address", "email.com")]
        [InlineData("Name", null, "Address", "email.com")]
        [InlineData("Name", "8093721840", null, "email.com")]
        [InlineData("Name", "8093721840", "Address", null)]
        public async Task UserWithNullRequiredValuesShouldNotBeInserted(string name, string phone, string address,
            string email)
        {
            //Arrange
            var userToInsert = new User()
            {
                Name = name,
                Phone = phone,
                Address = address,
                Email = email
            };

            //Act
            var userResultTask = _userService.CreateUser(userToInsert);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await userResultTask);
            Assert.True(ex.Message.Length > 0);
        }

        [Theory]
        [InlineData(100, UserType.Normal)]
        [InlineData(100, UserType.Premium)]
        [InlineData(100, UserType.SuperUser)]
        public async Task UserWithDifferentTypesShouldBeInsertedCorrectly(int money, UserType userType)
        {
            //Arrange
            var userToInsert = new User()
            {
                Name = "test1",
                Phone = "809345445",
                Address = "Somewhere",
                Email = "email@hotmail.com",
                Money = money,
                UserType = userType
            };

            //Act
            var userResult = await _userService.CreateUser(userToInsert);

            //Assert
            Assert.True(userResult.IsSuccessful);
        }

        [Theory]
        //Case when gif is 80%
        [InlineData(98, UserType.Normal)]
        // Case when gif is 12%
        [InlineData(105, UserType.Normal)]
        //Case when gif is 20%
        [InlineData(105, UserType.Premium)]
        //Case when gif is 200%
        [InlineData(120, UserType.SuperUser)]
        public async Task UserGifShouldBeCalculatedCorrectly(int money, UserType userType)
        {
            //Arrange
            var userToInsert = new User()
            {
                Name = "test1",
                Phone = "809345445",
                Address = "Somewhere",
                Email = "email@hotmail.com",
                Money = money,
                UserType = userType
            };
            var gif = getGif(userType, money);

            //Act
            var userResult = await _userService.CreateUser(userToInsert);

            //Assert
            var expected = userToInsert.Money + gif;
            Assert.True(userResult.DataResult.Money == expected);
        }

        [Theory]
        //Check duplicated phone or email
        [InlineData("Name", "+5491154762312", "Address", "Juan@marmol.com")]
        //Check duplicated name and address
        [InlineData("Franco", "809545644", "Alvear y Colombres", "email@.com")]
        public async Task NewDuplicatedUserShouldThrowAnException(string name, string phone, string address,
            string email)
        {
            //Arrange
            var newUser = new User()
            {
                Name = name,
                Phone = phone,
                Address = address,
                Email = email,
                Money = 100,
                UserType = UserType.Normal
            };

            //Act
            var userResultTask = _userService.CreateUser(newUser);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await userResultTask);
            Assert.True(ex.Message == $"{Messages.DuplicatedUser}{newUser.Name}");
        }
        
        private StreamReader getStreamReader()
        {
            var fileStream = new FileStream(Directory.GetCurrentDirectory() + "/Files/users.txt", FileMode.Open);

            var readerResult = new StreamReader(fileStream);

            return readerResult;
        }

        private decimal getGif(UserType userType, decimal money)
        {
            decimal gif = 0M;
            switch (userType)
            {
                //For normal user apply gif of 12% or 80% 
                case UserType.Normal:
                    if (money > 100) gif = money * 0.12M;
                    if (money <= 100 && money > 10) gif = money * 0.8M;
                    break;

                //For normal user apply gif of 20%
                case UserType.SuperUser:
                    if (money > 100) gif = money * 0.20M;
                    break;

                //For normal user apply gif of 200%
                case UserType.Premium:
                    if (money > 100) gif = money * 2M;
                    break;
            }
            return gif;
        }
    }
}