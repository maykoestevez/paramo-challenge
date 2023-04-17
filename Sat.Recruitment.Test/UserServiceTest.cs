using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Application.Services;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure.Interfaces;
using Sat.Recruitment.Infrastructure.Mappings;
using Sat.Recruitment.Infrastructure.ModelDtos;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserServiceTest
    {
        private readonly IUserService _userService;

        private readonly User _userToTest = new User("Pedro", "something@gmail.com", "Some where", "80945588");

        public UserServiceTest()
        {
            var userRepository = new Mock<IUserRepository>();
            var myProfile = new UserMappings();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            userRepository.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(_userToTest);
            _userService = new UserService(userRepository.Object, mapper);
        }

        [Fact]
        public async Task UserShouldBeInsertedCorrectly()
        {
            //Arrange
            var newUser = new UserDto
            {
                Name = "Pedro",
                Email = "something@gmail.com",
                Address = "Some where",
                Phone = "80945588"
            };

            //Act
            var createdUser = await _userService.CreateUser(newUser);

            //Assert
            Assert.NotNull(createdUser);
        }
    }
}