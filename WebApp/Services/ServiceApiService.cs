using AutoMapper;
using Shared.BL.DTOs;
using WebApp.Models;

namespace WebApp.Services
{
    public class ServiceApiService : IServiceApiService
    {

        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;

        public ServiceApiService(HttpClient httpClient, IMapper mapper, ApiFetchService apiFetchService)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }

        public Task<ServiceCreateVM> CreateService(ServiceCreateVM vm)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteService(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/service/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<HttpResponseMessage> EditService(ServiceEditVM vm)
        {
            vm.ServiceTypes = await _apiFetchService.FetchDataList<ServiceTypeDto, ServiceTypeVM>("api/servicetype?count=1000&start=0") ?? new List<ServiceTypeVM>();
            vm.Cities = await _apiFetchService.FetchDataList<CityDto, CityVM>("api/city?count=1000&start=0") ?? new List<CityVM>();
            var professionals = await _apiFetchService.FetchDataList<ProfessionalDto, ProfessionalVM>("api/professional?count=1000&start=0") ?? new List<ProfessionalVM>();
            var users = await _apiFetchService.FetchDataList<UserDto, UserVM>("api/user?count=1000&start=0");
            List<ProfessionalVM> professionalsData = professionals.Select(p => new ProfessionalVM
            {
                IdProfessional = p.IdProfessional,
                UserId = p.UserId,
                Cities = _mapper.Map<List<CityVM>>(p.Cities),
                UserName = users.FirstOrDefault(u => u.Iduser == p.UserId)?.Username
            }).ToList();

            vm.Professionals = professionalsData;

            var exists = vm.ServiceTypes.Any(st => st.ServiceTypeName == vm.SelectedServiceTypeName);


            var serviceTypes = await _apiFetchService.FetchDataList<ServiceTypeDto, ServiceTypeVM>("api/servicetype?count=1000&start=0") ?? new List<ServiceTypeVM>();
            var serviceId = serviceTypes.FirstOrDefault(st => st.ServiceTypeName == vm.SelectedServiceTypeName)?.IdserviceType;

            var serviceDto = new ServiceDto
            {
                IdService = vm.IdService,
                ProfessionalId = vm.SelectedProfessionalId,
                ServiceTypeId = serviceId,
                Description = vm.Description,
                Price = vm.Price
            };

            var response = await _httpClient.PutAsJsonAsync($"api/service/{vm.IdService}", serviceDto);

            return response;
        }

        public Task<ServiceCreateVM> GetCreateServiceView()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceEditVM> GetEditView()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetServiceIndex(int count, int start)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceSearchVM> SearchAsync(int? cityId, string serviceTypeName, int count = 1000, int start = 0)
        {
            throw new NotImplementedException();
        }
    }
}
