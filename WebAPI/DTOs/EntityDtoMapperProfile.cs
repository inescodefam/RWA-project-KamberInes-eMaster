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
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingDto, Booking>();
            CreateMap<Rating, RatingDto>();
            CreateMap<RatingDto, Rating>();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
        }
    }
}
