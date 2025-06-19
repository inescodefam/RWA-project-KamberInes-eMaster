using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ServiceTypeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;


        public ServiceTypeController(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }

        [HttpGet]
        public IActionResult CreateServiceType() => View();

        [HttpPost]
        public async Task<IActionResult> CreateServiceType(ServiceTypeVM model)
        {
            if (ModelState.IsValid)
            {
                var serviceTypeDto = _mapper.Map<ServiceTypeDto>(model);
                var response = await _httpClient.PostAsJsonAsync("api/servicetype", serviceTypeDto);

                return RedirectToAction("Search", "Service");
            }

            return View(model);
        }
    }
}
