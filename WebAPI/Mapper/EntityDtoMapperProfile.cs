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
            CreateMap<LogApiDto, LogDto>();
            CreateMap<LogDto, LogApiDto>();
            CreateMap<ProfessionalApiDto, ProfessionalDto>();
            CreateMap<ProfessionalDto, ProfessionalApiDto>();
            CreateMap<RoleApiDto, RoleDto>();
            CreateMap<RoleDto, RoleApiDto>();
            CreateMap<ServiceDto, ServiceApiDto>();
            CreateMap<ServiceApiDto, ServiceDto>();
            CreateMap<ServiceTypeApiDto, ServiceTypeDto>();
            CreateMap<ServiceTypeDto, ServiceTypeApiDto>();
            CreateMap<UserApiDto, UserDto>();
            CreateMap<UserDto, UserApiDto>();
        }
    }
}
