using AutoMapper;
using eProfessional.BLL.DTOs;


namespace WebAPI.DTOs
{
    public class EntityDtoMapperProfile : Profile
    {
        public EntityDtoMapperProfile()
        {
            CreateMap<ServiceDto, ServiceApiDto>();
            CreateMap<ServiceApiDto, ServiceDto>();
        }
    }
}
