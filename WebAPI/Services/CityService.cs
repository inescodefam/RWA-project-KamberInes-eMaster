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

        public async Task<CityDto> CreateCityAsync(string cityName)
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
            await _context.Cities.AddAsync(city);

            return _mapper.Map<CityDto>(city);
        }

        public Task<List<CityDto>> GetCitiesAsync(string searchTerm, int count, int start = 0)
        {
            var cities = _context.Cities
                .Where(c => string.IsNullOrEmpty(searchTerm) || c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Skip(start)
                .Take(count)
                .ToList();
            var cityDtos = _mapper.Map<List<CityDto>>(cities);

            return cities.Count == 0
                ? Task.FromResult(new List<CityDto>())
                : Task.FromResult(cityDtos);
        }
    }
}
