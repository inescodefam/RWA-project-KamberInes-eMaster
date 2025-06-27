using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]

    public class ServiceController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;
        private readonly IProfessionalService _professionalApiService;
        private readonly IUserService _userApiService;
        private readonly ICityProfessionalService _cityProfessionalService;

        private readonly IServiceApiService _serviceApiService;


        public ServiceController(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService, IProfessionalService professionalApiService, IUserService userApiService, ICityProfessionalService cityProfessionalService, IServiceApiService serviceApiService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
            _professionalApiService = professionalApiService;
            _userApiService = userApiService;
            _cityProfessionalService = cityProfessionalService;
            _serviceApiService = serviceApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int count, int start = 0)
        {
            // coment when ui is done todo implement pagination
            count = 50;
            var response = await _httpClient.GetAsync($"api/service?count={count}&start={start}");
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> Search(int? cityId, string serviceTypeName)
        {
            int count = 1000; // todo remove when ...
            int start = 0;

            var cityDtos = await _httpClient.GetFromJsonAsync<List<CityDto>>($"api/city?count={count}&start={start}") ?? new List<CityDto>();
            var serviceTypeDtos = await _httpClient.GetFromJsonAsync<List<ServiceTypeDto>>($"api/servicetype?count={count}&start={start}") ?? new List<ServiceTypeDto>();
            var serviceDtos = await _httpClient.GetFromJsonAsync<List<ServiceDto>>($"api/service?count={count}&start={start}") ?? new List<ServiceDto>();
            var usersDtos = await _userApiService.GetUsers(count, start);
            var professionalsDtos = await _professionalApiService.GetProfessionals(count);
            var cityProfessionalDtos = await _cityProfessionalService.GetCityProfessionalsAsync();

            var cities = _mapper.Map<List<CityVM>>(cityDtos)?
              .Where(c => c != null && !string.IsNullOrEmpty(c.Name))
              .ToList() ?? new List<CityVM>();

            var serviceTypes = _mapper.Map<List<ServiceTypeVM>>(serviceTypeDtos)?
               .Where(st => st != null && !string.IsNullOrEmpty(st.ServiceTypeName))
               .ToList() ?? new List<ServiceTypeVM>();

            var serviceResult = new List<ServiceResultVM>();

            //Dictionary<int, List<string>> listCitiesNames = new Dictionary<int, List<string>>();
            //foreach (var city in cityProfessionalDtos)
            //{
            //    listCitiesNames[city.ProfessionalId].Add(cities.FirstOrDefault(c => c.Idcity == city.CityId)?.Name ?? "Unknown City");
            //}
            var listCitiesNames = new Dictionary<int, List<string>>();

            var cityNameLookup = cities.ToDictionary(c => c.Idcity, c => c.Name);

            foreach (var cp in cityProfessionalDtos)
            {
                if (cp.ProfessionalId == null) continue;

                int professionalId = cp.ProfessionalId;

                if (!listCitiesNames.ContainsKey(professionalId))
                {
                    listCitiesNames[professionalId] = new List<string>();
                }

                var cityName = cp.CityId.HasValue && cityNameLookup.TryGetValue(cp.CityId.Value, out var name)
                    ? name
                    : "Unknown City";

                listCitiesNames[professionalId].Add(cityName);
            }

            foreach (ServiceDto s in serviceDtos)
            {
                int professionalUserId = professionalsDtos.FirstOrDefault(p => p.IdProfessional == s.ProfessionalId)?.UserId ?? 0;

                ProfessionalDto pro = professionalsDtos.FirstOrDefault(p => p.IdProfessional == s.ProfessionalId);
                List<string> cityNames = listCitiesNames.ContainsKey(pro.IdProfessional)
                                 ? listCitiesNames[pro.IdProfessional]
                                 : new List<string>();

                var serviceResultVM = new ServiceResultVM
                {
                    IdService = s.IdService,
                    ProfessionalName = usersDtos.FirstOrDefault(u => u.Iduser == professionalUserId).Username,
                    CityNames = cityNames,
                    ServiceTypeName = serviceTypes.FirstOrDefault(st => st.IdserviceType == s.ServiceTypeId)?.ServiceTypeName,
                    Description = s.Description,
                    Price = s.Price
                };
                serviceResult.Add(serviceResultVM);
            }

            // todo filtering logic....
            var filteredServices = serviceResult?
              .Where(s =>
                  (!cityId.HasValue ||
                   (cities?.Any(c => c.Idcity == cityId) ?? false)) &&
                  (string.IsNullOrEmpty(serviceTypeName) ||
                   s.ServiceTypeName == serviceTypeName)
              )
              .ToList() ?? new List<ServiceResultVM>();


            var vm = new ServiceSearchVM
            {
                Cities = cities ?? new List<CityVM>(),
                ServiceTypes = serviceTypes ?? new List<ServiceTypeVM>(),
                SelectedCityId = cityId,
                SelectedServiceTypeName = serviceTypeName,
                Services = filteredServices ?? new List<ServiceResultVM>()
            };

            if (vm.ServiceTypes == null)
                vm.ServiceTypes = new List<ServiceTypeVM>();

            if (vm.Cities == null)
                vm.Cities = new List<CityVM>();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var serviceTypes = await _apiFetchService.FetchDataList<ServiceTypeDto, ServiceTypeVM>("api/servicetype?count=1000&start=0") ?? new List<ServiceTypeVM>();
            var cities = await _apiFetchService.FetchDataList<CityDto, CityVM>("api/city?count=1000&start=0") ?? new List<CityVM>();
            var professionalDtos = await _professionalApiService.GetProfessionals(1000);
            var cityProfessionals = await _cityProfessionalService.GetCityProfessionalsAsync();

            var userDtos = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/user?count=1000&start=0")
                ?? new List<UserDto>();

            var professionals = professionalDtos.Select(p => new ProfessionalVM
            {
                IdProfessional = p.IdProfessional,
                UserId = p.UserId,
                Cities = _mapper.Map<List<CityVM>>(p.Cities),
                UserName = userDtos.FirstOrDefault(u => u.Iduser == p.UserId)?.Username
            }).ToList();

            var vm = new ServiceCreateVM
            {
                ServiceTypes = serviceTypes,
                Cities = cities,
                Professionals = professionals ?? new List<ProfessionalVM>(),

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceCreateVM vm)
        {
            var serviceTypes = await _apiFetchService.FetchDataList<ServiceTypeDto, ServiceTypeVM>("api/servicetype?count=1000&start=0") ?? new List<ServiceTypeVM>();
            var cities = await _apiFetchService.FetchDataList<CityDto, CityVM>("api/city?count=1000&start=0") ?? new List<CityVM>();
            var professionalDtos = await _httpClient.GetFromJsonAsync<List<ProfessionalDto>>("api/professional?count=1000&start=0") ?? new List<ProfessionalDto>();
            var userDtos = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/user?count=1000&start=0")
                ?? new List<UserDto>();

            if (!ModelState.IsValid)
            {
                vm.Professionals = professionalDtos.Select(p => new ProfessionalVM
                {
                    IdProfessional = p.IdProfessional,
                    UserId = p.UserId,
                    Cities = _mapper.Map<List<CityVM>>(p.Cities),
                    UserName = userDtos.FirstOrDefault(u => u.Iduser == p.UserId)?.Username
                }).ToList();
                vm.ServiceTypes = serviceTypes;
                vm.Cities = cities;

                return View(vm);
            }

            var serviceTypeDtos = await _httpClient.GetFromJsonAsync<List<ServiceTypeDto>>("api/servicetype?count=1000&start=0")
                 ?? new List<ServiceTypeDto>();

            var serviceTypeId = serviceTypeDtos
                .FirstOrDefault(st => st.ServiceTypeName == vm.SelectedServiceTypeName)
                ?.IdserviceType;

            var serviceDto = new ServiceDto
            {
                ProfessionalId = vm.SelectedProfessionalId,
                ServiceTypeId = serviceTypeId,
                Description = vm.Description,
                Price = vm.Price,
            };


            var response = await _httpClient.PostAsJsonAsync("api/service", serviceDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Search");
            }
            ModelState.AddModelError("", "Failed to create service. Please try again.");
            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var serviceDto = await _httpClient.GetFromJsonAsync<ServiceDto>($"api/service/id/{id}");

            var serviceTypes = await _apiFetchService.FetchDataList<ServiceTypeDto, ServiceTypeVM>("api/servicetype?count=1000&start=0") ?? new List<ServiceTypeVM>();
            var cities = await _apiFetchService.FetchDataList<CityDto, CityVM>("api/city?count=1000&start=0") ?? new List<CityVM>();
            var professionals = await _httpClient.GetFromJsonAsync<List<ProfessionalDto>>("api/professional?count=1000&start=0") ?? new List<ProfessionalDto>();
            var users = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/user?count=1000&start=0") ?? new List<UserDto>();
            var cityProfessional = await _cityProfessionalService.GetCityProfessionalsAsync();

            List<int> cityId = cityProfessional.Where(cp => cp.ProfessionalId == serviceDto.ProfessionalId && cp.CityId.HasValue).Select(cp => cp.CityId.Value).ToList();

            List<ProfessionalVM> professionalsData = professionals.Select(p => new ProfessionalVM
            {
                IdProfessional = p.IdProfessional,
                UserId = p.UserId,
                Cities = _mapper.Map<List<CityVM>>(p.Cities),
                UserName = users.FirstOrDefault(u => u.Iduser == p.UserId)?.Username
            }).ToList();

            var vm = new ServiceEditVM
            {
                IdService = serviceDto.IdService,
                SelectedProfessionalId = serviceDto.ProfessionalId,
                SelectedCityId = cityId,
                SelectedServiceTypeName = serviceTypes.FirstOrDefault(st => st.IdserviceType == serviceDto.ServiceTypeId).ServiceTypeName,
                Description = serviceDto.Description,
                Price = serviceDto.Price,
                Professionals = professionalsData,
                Cities = cities,
                ServiceTypes = serviceTypes
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var response = await _serviceApiService.EditService(vm);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Search");

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _serviceApiService.DeleteService(id);

            return RedirectToAction("Search");
        }

        public IActionResult YourServices()
        {
            return View();
        }

    }

}
