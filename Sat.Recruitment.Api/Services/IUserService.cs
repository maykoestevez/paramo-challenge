using System.Threading.Tasks;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Services
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
        public Task<Result<User>> CreateUser(User user);
    }
}