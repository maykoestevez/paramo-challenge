using AutoMapper;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure.ModelDtos;

namespace Sat.Recruitment.Infrastructure.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}