﻿using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.DAL.Models;


namespace WebAPI.DTOs
{
    public class EntityDtoMapperProfileBLL : Profile
    {
        public EntityDtoMapperProfileBLL()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Professional, ProfessionalDto>();
            CreateMap<ProfessionalDto, Professional>();
            CreateMap<ProfessionalDataDto, Professional>();
            CreateMap<Professional, ProfessionalDataDto>();
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
            CreateMap<Log, LogDto>();
            CreateMap<LogDto, Log>();
        }
    }
}
