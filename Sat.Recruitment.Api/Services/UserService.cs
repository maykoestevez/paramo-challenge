using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Services
{
    /// <summary>
    /// User service to manage logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IFileService _fileService;

        public UserService(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <inheritdoc />
        public async Task<Result<User>> CreateUser(User user)
        {
            var UserValidationResult = ValidateUser(user);
            if (!UserValidationResult.IsSuccessful)
            {
                throw new ArgumentException(UserValidationResult.Errors);
            }

            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                UserType = user.UserType,
                Money = user.Money
            };

            var gifResult = CalculateGif(user.Money, user.UserType);
            newUser.Money += gifResult.IsSuccessful ? gifResult.DataResult : 0M;

            var usersFromFileResult = await GetUsersFromFile(newUser);

            if (!usersFromFileResult.IsSuccessful)
            {
                throw new AggregateException(usersFromFileResult.Errors);
            }

            var userIsDuplicated = usersFromFileResult.DataResult.Any(u =>
                u.Email == newUser.Email || u.Phone == newUser.Phone || u.Name == newUser.Name &&
                u.Address == newUser.Address);

            if (userIsDuplicated) throw new ArgumentException($"{Messages.DuplicatedUser}{newUser.Name}");

            return new Result<User>() {IsSuccessful = true, DataResult = newUser};
        }

        /// <summary>
        /// Normalize email checking for invalid character and removing it
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private string NormalizeUserEmail(string email)
        {
            var emailSplit = email.Split(new char[] {'@'}, StringSplitOptions.RemoveEmptyEntries);
            var indexToRemove = emailSplit[0].IndexOf("+", StringComparison.Ordinal);
            emailSplit[0] = indexToRemove < 0
                ? emailSplit[0].Replace(".", "")
                : emailSplit[0].Replace(".", "").Replace("+","");

            return string.Join("@", new string[] {emailSplit[0], emailSplit[1]});
        }

        /// <summary>
        /// Get users from local file
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<Result<IList<User>>> GetUsersFromFile(User user)
        {
            var users = new List<User>();
            var reader = _fileService.ReadUsersFromFile();

            user.Email = NormalizeUserEmail(user.Email);

            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                var newUser = new User
                {
                    Name = line.Split(',')[0],
                    Email = line.Split(',')[1],
                    Phone = line.Split(',')[2],
                    Address = line.Split(',')[3],
                    UserType = Enum.Parse<UserType>(line.Split(',')[4]),
                    Money = decimal.Parse(line.Split(',')[5]),
                };

                users.Add(newUser);
            }

            reader.Close();

            return new Result<IList<User>>() {DataResult = users, IsSuccessful = true};
        }

        /// <summary>
        /// Validate required filed form new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Result<string> ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return new Result<string>() {Errors = Messages.NameRequired};
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return new Result<string>() {Errors = Messages.EmailRequired};
            }

            if (string.IsNullOrEmpty(user.Address))
            {
                return new Result<string>() {Errors = Messages.AddressRequired};
            }

            if (string.IsNullOrEmpty(user.Phone))
            {
                return new Result<string>() {Errors = Messages.PhoneRequired};
            }

            return new Result<string>() {IsSuccessful = true};
        }

        /// <summary>
        /// Calculate gif base on money and user type
        /// </summary>
        /// <param name="money"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private Result<decimal> CalculateGif(decimal money, UserType userType)
        {
            var gif = 0.0M;
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
                default:
                    throw new ArgumentOutOfRangeException(Messages.UserTypeError);
            }

            return new Result<decimal>() {DataResult = gif, IsSuccessful = true};
        }
    }
}