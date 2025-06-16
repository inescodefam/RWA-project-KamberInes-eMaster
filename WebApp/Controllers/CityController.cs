using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;


namespace WebApp.Controllers
{
    public class CityController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;
        public CityController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpClient.BaseAddress = new Uri("http://localhost:5020/");

        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm, int page = 1, int pageSize = 10)
        {
            int start = (page - 1) * pageSize;
            string url = $"api/city?count={pageSize}&start={start}";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                url += $"&searchTerm={searchTerm}";
            }
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var cities = await response.Content.ReadFromJsonAsync<List<CityVM>>() ?? new List<CityVM>();
                var model = new CityIndexVM { Cities = cities };
                return View(model);
            }
            return View(new CityIndexVM { Cities = new List<CityVM>() });

        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityIndexVM model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/city", model.NewCityName);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to add city. API says: {error}");
            return RedirectToAction("Index");
        }

    }
}
