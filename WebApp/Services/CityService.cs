using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class CityService : ICityService
    {
        private readonly ApiService _apiService;

        public CityService(ApiService api)
        {
            _apiService = api;
        }
        public List<CityVM> GetCities(string searchTerm, int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int start = (page - 1) * pageSize;
            string url = $"api/city?count={pageSize}&start={start}";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                url += $"&searchTerm={searchTerm}";
            }
            var response = _apiService.FetchDataList<CityApiDto, CityVM>(url);
            if (response == null || !response.Any())
            {
                return new List<CityVM>();
            }

            return response;
        }

        public CityVM CreateCity(string cityName)
        {
            var response = _apiService.PostData<CityApiDto, CityVM>("api/city", new CityVM { Name = cityName });
            if (response == null)
            {
                return null;
            }
            return response;
        }

        public List<CityVM> GetAllCities()
        {
            var response = _apiService.FetchDataList<CityApiDto, CityVM>("api/city/all");
            if (response == null || !response.Any())
            {
                return new List<CityVM>();
            }
            return response;
        }

        public bool UpdateCity(int id, string name)
        {
            var response = _apiService.PutData<CityApiDto, CityVM>("api/city", new CityVM { Idcity = id, Name = name });

            if (response == null)
            {
                return false;
            }

            return true;
        }

        public bool DeleteCity(int id)
        {
            var response = _apiService.DeleteData($"api/city/{id}");
            if (!response)
            {
                return false;
            }
            return true;
        }
    }
}
