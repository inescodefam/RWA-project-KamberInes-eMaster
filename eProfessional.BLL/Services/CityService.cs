using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.BLL.Services
{
    public class CityService : ICityService
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;

        public CityService(IMapper mapper, ICityRepository cityRepository)
        {
            _mapper = mapper;
            _cityRepository = cityRepository;
        }

        public CityDto CreateCity(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException("City name cannot be null or empty.", nameof(cityName));
            }

            if (_cityRepository.GetIdByName(cityName) != -1)
            {
                throw new InvalidOperationException("City already exists.");
            }

            City city = new City
            {
                Name = cityName
            };

            try
            {
                _cityRepository.Add(city);
                _cityRepository.Save();
            }
            catch (Exception EX)
            {

                throw new InvalidOperationException($"City {cityName} can not be added");
            }
            CityDto cityDto = _mapper.Map<CityDto>(city);

            return cityDto;
        }

        public List<CityDto> SearchCities(string? searchTerm, int count, int start = 0)
        {
            var result = _cityRepository.Search(searchTerm, count, start);
            var ciiyDtos = _mapper.Map<List<CityDto>>(result);
            return ciiyDtos;
        }

        public List<CityDto> GetAllCities()
        {
            var cities = _cityRepository.GetAll();
            var citiesDtos = _mapper.Map<List<CityDto>>(cities);
            return citiesDtos;
        }

        public bool UpdateCity(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("City name cannot be null or empty.", nameof(name));
            }

            var city = _cityRepository.GetById(id);
            if (city == null)
            {
                throw new InvalidOperationException($"City with id {id} does not exsist");
            }
            if (_cityRepository.GetIdByName(name) != -1 && _cityRepository.GetIdByName(name) != id)
            {
                throw new InvalidOperationException($"City with name {name} already exists");
            }

            city.Name = name;
            try
            {
                _cityRepository.Update(city);
                _cityRepository.Save();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"City {name} can not be updated", ex);
            }
            return true;
        }

        public bool DeleteCity(int id)
        {
            var city = _cityRepository.GetById(id);
            if (city == null)
            {
                throw new InvalidOperationException($"City with id  {id}  does not exsist");
            }
            try
            {
                _cityRepository.Delete(city);
                _cityRepository.Save();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"City with id {id} can not be deleted", ex);
            }
            return true;
        }
    }
}
