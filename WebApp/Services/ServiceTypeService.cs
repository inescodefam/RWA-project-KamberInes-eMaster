using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Services;

namespace WebApp.Services
{
    public class ServiceTypeService : IServiceType
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ServiceTypeService(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public async Task<ServiceTypeDto> CreateServiceType(ServiceTypeDto model)
        {
            var serviceTypesDto = await GetServiceTypes(100, 0);

            if (serviceTypesDto.Any(st => st.ServiceTypeName.Equals(model.ServiceTypeName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Service type with the same name already exists.");
            }
            var serviceTypeDto = _mapper.Map<ServiceTypeDto>(model);
            var response = await _httpClient.PostAsJsonAsync("api/servicetype", serviceTypeDto);

            return serviceTypeDto;
        }

        public Task<bool> DeleteServiceType(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceTypeDto> GetServiceTypeById(int id)
        {

            throw new NotImplementedException();
        }

        public async Task<List<ServiceTypeDto>> GetServiceTypes(int count, int start)
        {
            var serviceTypes = await _httpClient.GetFromJsonAsync<List<ServiceTypeDto>>($"api/servicetype?count={count}&start={start}");
            return serviceTypes ?? new List<ServiceTypeDto>();
        }

        public Task<ServiceTypeDto> UpdateServiceType(ServiceTypeDto serviceTypeDto)
        {
            throw new NotImplementedException();
        }
    }
}
