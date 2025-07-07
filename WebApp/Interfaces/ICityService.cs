using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface ICityService
    {
        public List<CityVM> GetCities(string searchTerm, int page, int pageSize = 0);
        public CityVM CreateCity(string cityName);

        public List<CityVM> GetAllCities();

        public int GetCityCount(string searchTerm);
        public bool UpdateCity(int id, string name);
        bool DeleteCity(int id);
    }
}
