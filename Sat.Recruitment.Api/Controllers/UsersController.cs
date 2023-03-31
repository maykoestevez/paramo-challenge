using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;

namespace Sat.Recruitment.Api.Controllers
{
    /// <summary>
    /// Manage users request 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Handle user creation by getting user parameters
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("create-user")]
        public async Task<Result<User>> CreateUser(User user)
        {
          return await _userService.CreateUser(user);
        }
    }
}