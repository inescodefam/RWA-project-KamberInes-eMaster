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
        private readonly ICityProfessionalService _cpService;
        private readonly IProfessionalService _professionalService;
        private readonly IServiceType _serviceType;

        public ServiceService(
            IMapper mapper,
            ApiService apiFetchService,
            ICityProfessionalService cpService,
            IProfessionalService pofessionalService,
            IServiceType serviceType
            )
        {
            _mapper = mapper;
            _apiFetchService = apiFetchService;
            _cpService = cpService;
            _professionalService = pofessionalService;
            _serviceType = serviceType;
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

        public bool EditService(ServiceEditResultVM vm)
        {
            var serviceDto = new ServiceVM
            {
                IdService = vm.IdService,
                ProfessionalId = vm.SelectedProfessionalId,
                ServiceTypeId = vm.SelectedServiceTypeId,
                Description = vm.Description,
                Price = vm.Price
            };

            var response = _apiFetchService.PutData<ServiceApiDto, ServiceVM>($"api/service", serviceDto);

            return response != null;
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

        public ServiceEditVM GetServiceByIdEditVm(int id)
        {
            var response = GetServiceByID(id);
            var serviceEditVm = MapServiceResultToServiceEditViewModel(response);

            return serviceEditVm;
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
            var professionalsDataVM = _professionalService.GetSingleProfessional(service.ProfessionalId);
            var serviceTypesDataVM = _serviceType.GetServiceTypeById(service.ServiceTypeId);
            List<CityVM> cities = _cpService.GetCitysByProfessional(service.ProfessionalId);
            List<string> cityNames = cities.Select(c => c.Name).ToList();

            return new ServiceResultVM
            {
                IdService = service.IdService,
                Description = service.Description,
                Price = service.Price,
                ProfessionalName = professionalsDataVM.UserName,
                ServiceTypeName = serviceTypesDataVM.ServiceTypeName,
                CityNames = cityNames
            };
        }

        private ServiceEditVM MapServiceResultToServiceEditViewModel(ServiceResultVM response)
        {
            var professionalIndexVm = _professionalService.GetProfessionals(1000, 0);

            int professionalId = professionalIndexVm.Professionals.FirstOrDefault(p => p.UserName == response.ProfessionalName)?.IdProfessional ?? 0;

            List<CityVM> cities = _cpService.GetCitysByProfessional(professionalId);

            int serviceTypeId = _serviceType.GetServiceTypes(1000, 0).FirstOrDefault(st => st.ServiceTypeName == response.ServiceTypeName).IdserviceType;


            List<int> cityIds = cities.Select(c => c.Idcity).ToList();

            ServiceEditVM serviceEditVM = new ServiceEditVM
            {
                IdService = response.IdService,
                SelectedProfessionalId = professionalId,
                SelectedCityId = cityIds,
                SelectedServiceTypeId = serviceTypeId,
                Description = response.Description,
                Price = response.Price,
                Professionals = professionalIndexVm.Professionals,
                Cities = cities,
                ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
            };

            return serviceEditVM;
        }

    }
}
