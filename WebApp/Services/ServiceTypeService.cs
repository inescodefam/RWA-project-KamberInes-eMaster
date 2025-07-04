using AutoMapper;
using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class ServiceTypeService : IServiceType
    {
        private readonly IMapper _mapper;
        private readonly ApiService _apiFetchService;

        public ServiceTypeService(IMapper mapper, ApiService apiFetchService)
        {
            _apiFetchService = apiFetchService;
            _mapper = mapper;
        }

        public ServiceTypeVM CreateServiceType(ServiceTypeVM model)
        {
            try
            {
                var response = _apiFetchService.PostData<ServiceTypeApiDto, ServiceTypeVM>("api/servicetype", model);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool DeleteServiceType(int id)
        {
            var response = _apiFetchService.DeleteData($"api/servicetype/{id}");
            return response;
        }

        public ServiceTypeVM GetServiceTypeById(int id)
        {
            var response = _apiFetchService.Fetch<ServiceTypeApiDto, ServiceTypeVM>($"api/servicetype/{id}");
            if (response != null)
            {
                return response;
            }
            else
            {
                throw new Exception("Service type not found.");
            }
        }

        public List<ServiceTypeVM> GetServiceTypes(int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var start = (page - 1) * pageSize;
            var serviceTypes = _apiFetchService.FetchDataList<ServiceTypeApiDto, ServiceTypeVM>($"api/servicetype?count={pageSize}&start={start}");
            return serviceTypes ?? new List<ServiceTypeVM>();
        }

        public ServiceTypeVM UpdateServiceType(ServiceTypeVM serviceTypeDto)
        {
            var response = _apiFetchService.PutData<ServiceTypeApiDto, ServiceTypeVM>("api/servicetype", serviceTypeDto);
            if (response != null)
            {
                return response;
            }
            else
            {
                throw new Exception("Failed to update service type.");
            }
        }
    }
}
