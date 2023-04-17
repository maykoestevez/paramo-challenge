using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure.ModelDtos;

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
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var userResult = await _userService.CreateUser(user);
            if (!userResult.IsSuccessful) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(userResult.DataResult);
        }
    }
}