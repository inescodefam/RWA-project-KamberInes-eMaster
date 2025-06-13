using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;

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
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            CreateMap<ServiceType, ServiceTypeDto>();
            CreateMap<ServiceTypeDto, ServiceType>();
        }
    }
}
