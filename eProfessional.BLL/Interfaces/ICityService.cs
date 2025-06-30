using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface ICityService
    {
        public List<CityDto> SearchCities(string? searchTerm, int count, int start = 0);
        public CityDto CreateCity(string cityName);

        public List<CityDto> GetAllCities();
        public bool UpdateCity(CityDto cityDto);
        bool DeleteCity(int id);
    }
}
