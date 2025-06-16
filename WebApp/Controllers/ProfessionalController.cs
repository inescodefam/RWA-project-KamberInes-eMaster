using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ProfessionalController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;

        public ProfessionalController(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }


        // GET: ProfessionalController
        public async Task<IActionResult> Index(int count, int start = 0)
        {
            // coment when ui is done todo create pagination
            count = 50;
            var professionalsResult = await _apiFetchService.FetchList<ProfessionalDto, ProfessionalVM>($"api/professional?count={count}&start={start}", this);

            if (professionalsResult is ViewResult viewResult && viewResult.Model is List<ProfessionalVM> professionals)
            {

                var users = await _apiFetchService.FetchList<UserDto, UserVM>("api/user?count=1000&start=0", this);
                var cities = await _apiFetchService.FetchList<CityDto, CityVM>("api/city?count=1000&start=0", this);

                var userList = (users as ViewResult)?.Model as List<UserVM> ?? new List<UserVM>();
                var cityList = (cities as ViewResult)?.Model as List<CityVM> ?? new List<CityVM>();

                var userDict = userList.ToDictionary(u => u.Id, u => u.Username);
                var cityDict = cityList.ToDictionary(c => c.Id, c => c.Name);

                foreach (var p in professionals)
                {
                    if (p.UserId.HasValue && userDict.TryGetValue((int)p.UserId, out var userName))
                        p.UserName = userName;
                    else
                        p.UserName = "Unknown";
                    if (p.CityId.HasValue && cityDict.TryGetValue(p.CityId.Value, out var cityName))
                        p.CityName = cityName;
                    else
                        p.CityName = "Unknown";
                }

                return View(professionals);
            }
            return professionalsResult;
        }

        public ActionResult Details(int id)
        {
            var professionals = _apiFetchService.FetchList<ProfessionalDto, ProfessionalVM>($"api/professional/{id}", this);
            return View(professionals);
        }

        // GET: ProfessionalController/Create
        // BecomeProfessional
        public async Task<IActionResult> CreateCity()
        {
            var cities = await _httpClient.GetFromJsonAsync<List<CityVM>>("api/city");
            var vm = new BecomeProfessionalVM { Cities = cities };
            return View(vm);
        }

        // POST: ProfessionalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BecomeProfessionalVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Cities = await _httpClient.GetFromJsonAsync<List<CityVM>>("api/city");
                    return View(model);
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var response = await _httpClient.PostAsJsonAsync("api/professional", new { UserId = userId, CityId = model.CityId });
                //var response = await _httpClient.PostAsync("api/professional",
                //new StringContent(JsonSerializer.Serialize(new { UserId = userId, CityId = model.CityId }), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index", "Service");
                }

                ModelState.AddModelError("", "Could not become professional.");
                return View();
            }
            catch
            {

                model.Cities = await _httpClient.GetFromJsonAsync<List<CityVM>>("api/city");
                return View(model);
            }
        }

        // GET: ProfessionalController/Edit/5
        public ActionResult Edit(int id)
        {
            var professional = _httpClient.GetAsync($"api/professional/{id}");
            return View(professional);
        }

        // POST: ProfessionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                _httpClient.PutAsync($"api/professional/{id}",
                    new StringContent(JsonSerializer.Serialize(collection), Encoding.UTF8, "application/json"));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfessionalController/Delete/5
        public ActionResult Delete(int id)
        {
            var professional = _httpClient.GetAsync($"api/professional/{id}");
            return View(professional);
        }

        // POST: ProfessionalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _httpClient.DeleteAsync($"api/professional/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
