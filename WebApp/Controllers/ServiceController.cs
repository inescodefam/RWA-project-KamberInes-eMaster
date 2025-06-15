using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ServiceController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;


        public ServiceController(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }

        public async Task<IActionResult> Index(int count, int start = 0)
        {
            // coment when ui is done
            count = 50;
            return await _apiFetchService.FetchList<ServiceDto, ServiceVM>($"api/service?count={count}&start={start}", this);
        }

        public async Task<ActionResult> Search(int? cityId, string serviceTypeName)
        {
            var cityDtos = await _httpClient.GetFromJsonAsync<List<CityDto>>("api/city?count=1000&start=0");
            var serviceTypeDtos = await _httpClient.GetFromJsonAsync<List<ServiceTypeDto>>("api/servicetype?count=1000&start=0");
            var serviceDtos = await _httpClient.GetFromJsonAsync<List<ServiceDto>>("api/service?count=1000&start=0");

            var cities = _mapper.Map<List<CityVM>>(cityDtos);
            var serviceTypes = _mapper.Map<List<ServiceTypeVM>>(serviceTypeDtos);
            var allServices = _mapper.Map<List<ServiceResultVM>>(serviceDtos);

            var filteredServices = allServices
                .Where(s =>
                    (!cityId.HasValue || s.CityName == cities.FirstOrDefault(c => c.id == cityId)?.Name) &&
                    (string.IsNullOrEmpty(serviceTypeName) || s.ServiceTypeName == serviceTypeName)
                )
                .ToList();

            var vm = new ServiceSearchVM
            {
                Cities = cities,
                ServiceTypes = serviceTypes,
                SelectedCityId = cityId,
                SelectedServiceTypeName = serviceTypeName,
                Services = filteredServices

            };

            return View(vm);
        }
    }
}
