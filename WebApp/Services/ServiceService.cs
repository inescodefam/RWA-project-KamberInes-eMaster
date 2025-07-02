using AutoMapper;
using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class ServiceService : IServiceService
    {

        private readonly IMapper _mapper;
        private readonly ApiService _apiFetchService;

        public ServiceService(HttpClient httpClient, IMapper mapper, ApiService apiFetchService)
        {
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }

        public ServiceCreateVM CreateService(ServiceCreateVM vm)
        {
            ServiceVM serviceVM = new ServiceVM
            {
                ProfessionalId = vm.SelectedProfessionalId,
                ServiceTypeId = vm.SelectedServiceTypeId,
                Description = vm.Description,
                Price = vm.Price
            };
            var response = _apiFetchService.PostData<ServiceApiDto, ServiceVM>("api/service", serviceVM);
            if (response != null)
            {
                var createdVm = new ServiceCreateVM
                {
                    SelectedProfessionalId = vm.SelectedProfessionalId,
                    SelectedServiceTypeId = vm.SelectedServiceTypeId,
                    Description = vm.Description,
                    Price = vm.Price,
                    SelectedServiceTypeName = vm.SelectedServiceTypeName,
                    SelectedCitiesIds = vm.SelectedCitiesIds
                };
                return createdVm;
            }
            else
            {
                throw new Exception("Failed to create service. Please check the input data or your network connection.");
            }
        }

        public bool DeleteService(int id)
        {
            var response = _apiFetchService.DeleteData($"api/service/{id}");
            return response;
        }

        public void EditService(ServiceEditVM vm)
        {
            var serviceDto = new ServiceVM
            {
                IdService = vm.IdService,
                ProfessionalId = vm.SelectedProfessionalId,
                ServiceTypeId = vm.IdService,
                Description = vm.Description,
                Price = vm.Price
            };

            var response = _apiFetchService.PutData<ServiceApiDto, ServiceVM>($"api/service/{vm.IdService}", serviceDto);
        }

        public ServiceResultVM GetServiceByID(int id)
        {
            var url = $"api/service/id/{id}";
            var response = _apiFetchService.Fetch<ServiceApiDto, ServiceVM>(url);
            if (response == null)
            {
                throw new Exception($"Service with ID {id} not found.");
            }

            var serviceResultVM = MapServiceToResult(response);
            return serviceResultVM;
        }

        public List<ServiceResultVM> GetServiceIndex(int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var start = (page - 1) * pageSize;
            var url = $"api/service?count={pageSize}&start={start}";
            var response = _apiFetchService.FetchDataList<ServiceApiDto, ServiceVM>(url);

            if (response == null || !response.Any())
            {
                return new List<ServiceResultVM>();
                throw new Exception("No services found.");
            }
            var serviceResults = new List<ServiceResultVM>();

            foreach (var service in response)
            {
                if (service.ProfessionalId <= 0 || service.ServiceTypeId <= 0)
                {
                    return new List<ServiceResultVM>();
                }
                var serviceResultVM = MapServiceToResult(service);
                serviceResults.Add(serviceResultVM);
            }
            return serviceResults;
        }

        public List<ServiceResultVM> Search(string serviceTypeName, int count, int start = 0)
        {
            var url = $"api/service/search?serviceTypeName={serviceTypeName}&count={count}&start={start}";

            var response = _apiFetchService.FetchDataList<ServiceApiDto, ServiceVM>(url);

            if (response == null || !response.Any())
            {
                return new List<ServiceResultVM>();
            }
            var serviceResults = new List<ServiceResultVM>();
            foreach (var service in response)
            {
                var serviceResultVM = MapServiceToResult(service);
                serviceResults.Add(serviceResultVM);
            }
            return serviceResults;
        }

        // PRIVATE

        private ServiceResultVM MapServiceToResult(ServiceVM service)
        {
            var professionalsDataVM = _apiFetchService.Fetch<ProfessionalApiDataDto, ProfessionalDataVM>($"api/professional/{service.ProfessionalId}");
            var serviceTypesDataVM = _apiFetchService.Fetch<ServiceTypeApiDto, ServiceTypeVM>($"api/servicetype/{service.ServiceTypeId}");

            return new ServiceResultVM
            {
                IdService = service.IdService,
                Description = service.Description,
                Price = service.Price,
                ProfessionalName = professionalsDataVM.UserName,
                ServiceTypeName = serviceTypesDataVM.ServiceTypeName
            };
        }
    }
}
