using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using WebAPI.Context;

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

        public CityDto CreateCity(string cityName)
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
            _context.SaveChanges();

            City c = _context.Cities.FirstOrDefault(x => x.Name == cityName);
            CityDto cityDto = _mapper.Map<CityDto>(c);

            return cityDto;
        }

        public List<CityDto> GetCities(string searchTerm, int count, int start = 0)
        {

            var query = _context.Cities.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }

            var result = query.Skip(start).Take(count).ToList();
            var ciiyDtos = _mapper.Map<List<CityDto>>(result);
            return ciiyDtos;
        }

        public List<CityDto> GetAllCities()
        {
            var cities = _context.Cities.ToList();
            var citiesDtos = _mapper.Map<List<CityDto>>(cities);
            return citiesDtos;
        }

        public bool UpdateCity(int id, string name)
        {
            var city = _context.Cities.Find(id);
            if (city == null)
            {
                return false;
            }

            city.Name = name;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCity(int id)
        {
            var city = _context.Cities.Find(id);
            if (city == null)
            {
                return false;
            }
            _context.Cities.Remove(city);
            _context.SaveChanges();
            return true;
        }
    }
}
