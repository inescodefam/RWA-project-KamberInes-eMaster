using AutoMapper;
using WebAPI.DTOs;
using WebApp.Models;

namespace WebApp.Mapper
{
    public class EntityDtoMapperProfile : Profile
    {
        public EntityDtoMapperProfile()
        {
            CreateMap<UserVM, UserApiDto>();
            CreateMap<UserApiDto, UserVM>();

            CreateMap<ProfessionalApiDto, ProfessionalVM>();
            CreateMap<ProfessionalVM, ProfessionalApiDto>();
            CreateMap<ProfessionalApiDataDto, ProfessionalDataVM>();
            CreateMap<ProfessionalDataVM, ProfessionalApiDataDto>();
            CreateMap<CreateProfessionalApiDataDto, CreateProfessionalVM>();
            CreateMap<CreateProfessionalVM, CreateProfessionalApiDataDto>();

            CreateMap<CityProfessionalApiDto, CityProfessionalVM>();
            CreateMap<CityProfessionalVM, CityProfessionalApiDto>();

            CreateMap<ServiceTypeApiDto, ServiceTypeVM>();
            CreateMap<ServiceTypeVM, ServiceTypeApiDto>();

            CreateMap<CityApiDto, CityVM>();
            CreateMap<CityVM, CityApiDto>();

            CreateMap<ServiceVM, ServiceApiDto>();
            CreateMap<ServiceApiDto, ServiceVM>();

            CreateMap<RoleVM, RoleApiDto>();
            CreateMap<RoleApiDto, RoleVM>();

        }
    }
}
