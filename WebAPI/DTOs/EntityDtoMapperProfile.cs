using AutoMapper;
using WebAPI.Models;

namespace WebAPI.DTOs
{
    public class EntityDtoMapperProfile : Profile
    {
        public EntityDtoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Professional, ProfessionalDto>();
            CreateMap<ProfessionalDto, Professional>();
        }
    }
}
