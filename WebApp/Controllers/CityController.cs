using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CityController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CityController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public ActionResult<List<CityVM>> Index()
        {
            var response = _httpClient.GetAsync("api/city").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var cities = JsonSerializer.Deserialize<List<CityVM>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(cities);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var content = new StringContent(JsonSerializer.Serialize(model.Name), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/city", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create city.");
            return View(model);
        }
    }
}
