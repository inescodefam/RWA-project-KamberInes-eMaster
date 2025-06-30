using AutoMapper;
using eProfessional.BLL.DTOs;


namespace WebAPI.DTOs
{
    public class EntityDtoMapperProfile : Profile
    {
        public EntityDtoMapperProfile()
        {
            CreateMap<AuthApiDto, AuthDto>();
            CreateMap<AuthDto, AuthApiDto>();

            CreateMap<CityApiDto, CityDto>();
            CreateMap<CityDto, CityApiDto>();

            CreateMap<CityProfessionalApiDto, CityProfessionalDto>();
            CreateMap<CityProfessionalDto, CityProfessionalApiDto>();
            CreateMap<CreateCityProfessionalApiDto, CityProfessionalDto>();

            CreateMap<LogApiDto, LogDto>();
            CreateMap<LogDto, LogApiDto>();

            CreateMap<ProfessionalApiDto, ProfessionalDto>();
            CreateMap<ProfessionalDto, ProfessionalApiDto>();
            CreateMap<ProfessionalApiDataDto, ProfessionalDataDto>();
            CreateMap<ProfessionalDataDto, ProfessionalApiDataDto>();
            CreateMap<CreateProfessionalApiDataDto, ProfessionalDataDto>();
            CreateMap<ProfessionalDataDto, CreateProfessionalApiDataDto>();

            CreateMap<RoleApiDto, RoleDto>();
            CreateMap<RoleDto, RoleApiDto>();

            CreateMap<ServiceDto, ServiceApiDto>();
            CreateMap<ServiceApiDto, ServiceDto>();
            CreateMap<CreateServiceApiDto, ServiceDto>();
            CreateMap<ServiceDto, CreateServiceApiDto>();

            CreateMap<ServiceTypeApiDto, ServiceTypeDto>();
            CreateMap<ServiceTypeDto, ServiceTypeApiDto>();
            CreateMap<ServiceTypeDto, CreateServiceTypeApiDto>();
            CreateMap<CreateServiceTypeApiDto, ServiceTypeDto>();

            CreateMap<UserApiDto, UserDto>();
            CreateMap<UserDto, UserApiDto>();
        }
    }
}
