using Shared.BL.DTOs;
using Shared.BL.Services;

namespace WebApp.Services
{
    public class CityProfessionalService : ICityProfessionalService
    {

        private readonly HttpClient _httpClient;

        public CityProfessionalService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public Task<CityProfessionalDto> AddCityProfessionalAsync(CityProfessionalDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCityProfessionalAsync(int idProfessionalCity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CityProfessionalDto>> GetCityProfessionalsAsync()
        {
            var response = await _httpClient.GetAsync($"api/city-professional");
            List<CityProfessionalDto> cityProfessionalDtos = response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<CityProfessionalDto>>() ?? new List<CityProfessionalDto>()
                : new List<CityProfessionalDto>();
            return cityProfessionalDtos;
        }

        public Task<List<CityProfessionalDto?>> GetCitysByProfessionalAsync(int professionalId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CityProfessionalDto>> GetProfessionalsByCityAsync(int cityId)
        {
            throw new NotImplementedException();
        }

        public Task<CityProfessionalDto> UpdateCityProfessionalAsync(int id, CityProfessionalDto model)
        {
            throw new NotImplementedException();
        }
    }
}
