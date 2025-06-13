using AutoMapper;
using Shared.BL.DTOs;
using WebApp.Models;

namespace WebApp.Mapper
{
    public class EntityDtoMapperProfile : Profile
    {
        public EntityDtoMapperProfile()
        {
            CreateMap<UserVM, UserDto>();
            CreateMap<UserDto, UserVM>();
            CreateMap<ProfessionalDto, ProfessionalVM>();
            CreateMap<ProfessionalVM, ProfessionalDto>();
            CreateMap<ServiceTypeDto, ServiceTypeVM>();
            CreateMap<ServiceTypeVM, ServiceTypeDto>();
            CreateMap<CityDto, CityVM>();
            CreateMap<CityVM, CityDto>();
            CreateMap<ServiceVM, ServiceDto>();
            CreateMap<ServiceDto, ServiceVM>();
        }
    }
}
