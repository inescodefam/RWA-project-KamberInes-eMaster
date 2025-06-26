using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using WebAPI.Models;

namespace WebAPI.DTOs
{
    public class EntityDtoMapperProfile : Profile
    {
        public EntityDtoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Professional, ProfessionalDto>()
            .ForMember(dest => dest.Cities, opt => opt.Ignore());
            CreateMap<ProfessionalDto, Professional>();
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            CreateMap<ServiceType, ServiceTypeDto>();
            CreateMap<ServiceTypeDto, ServiceType>();
            CreateMap<CityProfessional, CityProfessionalDto>();
            CreateMap<CityProfessionalDto, CityProfessional>();
        }
    }
}
