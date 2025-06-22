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

        public ProfessionalApiServices(IHttpClientFactory httpClientFactory, ApiFetchService apiFetchService, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _apiFetchService = apiFetchService;
            _mapper = mapper;
        }

        async public Task<ProfessionalDto> GetSingleProfessional(int id)
        {
            ProfessionalDto profesionalDto = await _apiFetchService.Fetch<Professional, ProfessionalDto>($"api/professional/{id}");
            return profesionalDto;
        }

        public bool CreateProfessional(ProfessionalDto professionalDto)
        {
            var newProfessional = _mapper.Map<Professional>(professionalDto);
            var response = _httpClient.PostAsJsonAsync("api/professional", newProfessional);
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

            return professionalsDto;
        }


        public List<ProfessionalDto> SearchProfessionals(string? Name, string? cityName, int count, int start = 0)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProfessional(int id, ProfessionalDto professionalDto)
        {
            var resposne = _httpClient.PutAsync($"api/professional/{id}",
                   new StringContent(JsonSerializer.Serialize(professionalDto), Encoding.UTF8, "application/json"));
            return resposne != null;
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
