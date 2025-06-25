using Shared.BL.DTOs;
using Shared.BL.Services;
using System.Text;
using System.Text.Json;

namespace WebApp.Services
{
    public class CityApiService : ICityService
    {
        private readonly HttpClient _httpClient;

        public CityApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public async Task<List<CityDto>> GetCitiesAsync(string searchTerm, int page, int pageSize = 10)
        {
            int start = (page - 1) * pageSize;
            string url = $"api/city?count={pageSize}&start={start}";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                url += $"&searchTerm={searchTerm}";
            }
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var cities = await response.Content.ReadFromJsonAsync<List<CityDto>>() ?? new List<CityDto>();

                return cities;
            }

            return new List<CityDto>();
        }

        public async Task<CityDto> CreateCityAsync(string cityName)
        {
            var response = await _httpClient.PostAsJsonAsync("api/city", new { Name = cityName });

            if (response.IsSuccessStatusCode)
            {
                var cityDto = await response.Content.ReadFromJsonAsync<CityDto>();
                return cityDto;
            }

            return null;
        }

        public async Task<List<CityDto>> GetAllCitiesAsync()
        {
            var response = await _httpClient.GetAsync("api/city/all");
            if (response.IsSuccessStatusCode)
            {
                var cities = await response.Content.ReadFromJsonAsync<List<CityDto>>() ?? new List<CityDto>();
                return cities;
            }
            return new List<CityDto>();
        }

        public async Task<bool> UpdateCityAsync(int id, string name)
        {
            var content = new StringContent(
              JsonSerializer.Serialize(new { Name = name }),
                  Encoding.UTF8,
                 "application/json"
             );

            var response = await _httpClient.PutAsync($"api/city/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/city/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
