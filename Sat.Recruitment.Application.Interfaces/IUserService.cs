using System.Threading.Tasks;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure.Helpers;
using Sat.Recruitment.Infrastructure.ModelDtos;


namespace Sat.Recruitment.Application.Interfaces
{
    /// <summary>
    /// Manage users logic
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Create a new user and perform validations
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<Result<UserDto>> CreateUser(UserDto user);
    }
}