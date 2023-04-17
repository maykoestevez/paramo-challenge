using System.Threading.Tasks;
using AutoMapper;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure;
using Sat.Recruitment.Infrastructure.Helpers;
using Sat.Recruitment.Infrastructure.Interfaces;
using Sat.Recruitment.Infrastructure.ModelDtos;

namespace Sat.Recruitment.Application.Services
{
    /// <summary>
    /// User service to manage logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Result<UserDto>> CreateUser(UserDto userDto)
        {
            Helper.NormalizeEmail(userDto.Email);
            
            var user = _mapper.Map<User>(userDto);
            var addedUser = await _userRepository.Add(user);
            var resultUserDto = _mapper.Map<UserDto>(addedUser);
            
            return new Result<UserDto>() {IsSuccessful = true, DataResult = resultUserDto};
        }
    }
}