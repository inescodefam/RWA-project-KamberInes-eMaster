using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using Shared.BL.Services;
using System.Text;
using System.Text.Json;

namespace WebApp.Services
{
    public class ProfessionalApiServices : IProfessionalService
    {

        private readonly HttpClient _httpClient;
        private readonly ApiFetchService _apiFetchService;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IUserService _userService;
        private readonly ICityProfessionalService _cityProfessionalService;

        public ProfessionalApiServices(IHttpClientFactory httpClientFactory, ApiFetchService apiFetchService, IMapper mapper, ICityService cityService, IUserService userService, ICityProfessionalService cityProfessional)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _apiFetchService = apiFetchService;
            _mapper = mapper;
            _cityService = cityService;
            _userService = userService;
            _cityProfessionalService = cityProfessional;
        }

        async public Task<ProfessionalDto> GetSingleProfessional(int id)
        {
            var response = await _httpClient.GetAsync($"api/professional/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }
            var professionalDto = await response.Content.ReadFromJsonAsync<ProfessionalDto>();

            return professionalDto;
        }

        public async Task<bool> CreateProfessional(ProfessionalDto professionalDto)
        {
            var newProfessional = _mapper.Map<Professional>(professionalDto);
            var response = await _httpClient.PostAsJsonAsync("api/professional", newProfessional);
            return response != null;
        }


        public async Task<List<ProfessionalDto>> GetProfessionals(int count, int start = 0)
        {
            var response = await _httpClient.GetAsync($"api/professional?count={count}&start={start}");
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new List<ProfessionalDto>();
            }
            var professionalsDto = await response.Content.ReadFromJsonAsync<List<ProfessionalDto>>();

            return professionalsDto ?? new List<ProfessionalDto>();
        }


        public async Task<List<ProfessionalDto>> Search(string? Name, string? cityName, int count, int start)
        {
            var professinals = await GetProfessionals(count, start);
            List<CityDto> cities = await _cityService.GetCitiesAsync(cityName, 1, 1000);
            List<UserDto> users = _userService.GetUsers(1000).Result;
            var cityProfessionalDtos = await _cityProfessionalService.GetCityProfessionalsAsync();

            CityDto city = null;
            List<int> userIds = null;

            if (Name != null)
                userIds = users.Where(u => u.Username == Name).Select(u => u.Iduser).ToList();

            if (cityName != null)
                city = cities.Find(c => cityName == c.Name);

            List<ProfessionalDto> result = new List<ProfessionalDto>();

            if (city != null)
            {
                var professionalIds = cityProfessionalDtos
                    .Where(cp => cp.CityId == city.Idcity && cp?.ProfessionalId != null)
                    .Select(cp => cp.ProfessionalId)
                    .ToList();

                result = professinals.Where(p => professionalIds.Contains(p.IdProfessional)).ToList();

            }
            else
            {
                result = professinals;
            }


            if (userIds != null)
                result = professinals.Where(p => userIds.Contains(p.UserId ?? -1)).ToList();

            return _mapper.Map<List<ProfessionalDto>>(result);
        }

        public async Task<bool> UpdateProfessional(int id, ProfessionalDto professionalDto)
        {
            var resposne = await _httpClient.PutAsync($"api/professional/{id}",
                   new StringContent(JsonSerializer.Serialize(professionalDto), Encoding.UTF8, "application/json"));
            return resposne.IsSuccessStatusCode;
        }
        public bool DeleteProfessional(int id)
        {
            try
            {
                var response = _httpClient.DeleteAsync($"api/professional/{id}");


                return response != null;
            }
            catch
            {

                throw new Exception($"Error deleting professional with ID {id}. Please try again later.");
            }
        }
    }
}
