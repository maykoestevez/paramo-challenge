using System;
using Sat.Recruitment.Domain.Models;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserDomainTest
    {

        [Theory]
        [InlineData(null, "8093721840", "Address", "email.com")]
        [InlineData("Name", null, "Address", "email.com")]
        [InlineData("Name", "8093721840", null, "email.com")]
        [InlineData("Name", "8093721840", "Address", null)]
        public void UserWithNullRequiredValuesShouldNotBeCreated(string name, string phone, string address,
            string email)
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => new User(name, email, address, phone));
            Assert.True(ex.Message.Length > 0);
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
        public void UserGifShouldBeCalculatedCorrectly(int money, UserType userType)
        {
            //Arrange
            var userToInsert = new User("test1", "email@hotmail.com", "Somewhere", "809345445")
            {
                Money = money,
                UserType = userType
            };

            //Act
            userToInsert.CalculateUserGif();

            //Assert
            var expected = GetGif(userType, money);
            Assert.True(userToInsert.Money == expected);
        }

        private decimal GetGif(UserType userType, decimal money)
        {
            decimal gif = 0M;
            switch (userType)
            {
                case UserType.Normal:
                    if (money > 100) gif = money * 0.12M;
                    if (money <= 100 && money > 10) gif = money * 0.8M;
                    break;
                case UserType.SuperUser:
                    if (money > 100) gif = money * 0.20M;
                    break;
                case UserType.Premium:
                    if (money > 100) gif = money * 2M;
                    break;
            }

            return money += gif;
        }
    }
}