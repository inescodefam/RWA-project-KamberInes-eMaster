using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface ICityService
    {
        public Task<List<CityDto>> GetCitiesAsync(string searchTerm, int count, int start = 0);
        public Task<CityDto> CreateCityAsync(string cityName);

        public Task<List<CityDto>> GetAllCitiesAsync();
        public Task<bool> UpdateCityAsync(int id, string name);
        Task<bool> DeleteCityAsync(int id);
    }
}
