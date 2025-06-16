using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using WebAPI.Models;

namespace Shared.BL.Services
{
    public class CityService : ICityService
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;

        public CityService(EProfessionalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public CityDto CreateCityAsync(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException("City name cannot be null or empty.", nameof(cityName));
            }

            if (_context.Cities.Any(x => x.Name == cityName))
            {
                throw new InvalidOperationException("City already exists.");
            }
            var city = new City
            {
                Name = cityName,
            };
            _context.Cities.Add(city);

            return _mapper.Map<CityDto>(city);
        }

        public Task<List<CityDto>> GetCitiesAsync(string searchTerm, int count, int start = 0)
        {
            throw new NotImplementedException();
        }
    }
}
