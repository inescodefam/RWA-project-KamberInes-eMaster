using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Services;

namespace WebApp.Services
{
    public class CityApiService : ICityService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CityApiService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
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

    }
}
