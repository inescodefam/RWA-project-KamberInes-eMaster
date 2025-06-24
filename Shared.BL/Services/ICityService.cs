using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface ICityService
    {
        public Task<List<CityDto>> GetCitiesAsync(string searchTerm, int count, int start = 0);
        public Task<CityDto> CreateCityAsync(string cityName);
    }
}
