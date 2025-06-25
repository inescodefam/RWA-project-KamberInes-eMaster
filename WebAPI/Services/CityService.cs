using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            _context.SaveChanges();

            City c = _context.Cities.FirstOrDefault(x => x.Name == cityName);
            CityDto cityDto = _mapper.Map<CityDto>(c);

            return cityDto;
        }

        public async Task<List<CityDto>> GetCitiesAsync(string searchTerm, int count, int start = 0)
        {

            var query = _context.Cities.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }

            var result = await query.Skip(start).Take(count).ToListAsync();
            var ciiyDtos = _mapper.Map<List<CityDto>>(result);
            return ciiyDtos;
        }

        public async Task<List<CityDto>> GetAllCitiesAsync()
        {
            var cities = await _context.Cities.ToListAsync();
            var citiesDtos = _mapper.Map<List<CityDto>>(cities);
            return citiesDtos;
        }

        public async Task<bool> UpdateCityAsync(int id, string name)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return false;
            }

            city.Name = name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return false;
            }
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
