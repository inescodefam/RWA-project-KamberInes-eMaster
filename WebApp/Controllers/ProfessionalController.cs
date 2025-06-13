using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
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
            // coment when ui is done
            count = 50;
            var professionalsResult = await _apiFetchService.FetchList<ProfessionalDto, ProfessionalVM>($"api/professional?count={count}&start={start}", this);

            if (professionalsResult is ViewResult viewResult && viewResult.Model is List<ProfessionalVM> professionals)
            {
                // Fetch users and cities
                var users = await _apiFetchService.FetchList<UserDto, UserVM>("api/user?count=1000&start=0", this);
                var cities = await _apiFetchService.FetchList<CityDto, CityVM>("api/city?count=1000&start=0", this);

                // Convert to lists
                var userList = (users as ViewResult)?.Model as List<UserVM> ?? new List<UserVM>();
                var cityList = (cities as ViewResult)?.Model as List<CityVM> ?? new List<CityVM>();

                // Build lookup dictionaries
                var userDict = userList.ToDictionary(u => u.Id, u => u.Username);
                var cityDict = cityList.ToDictionary(c => c.id, c => c.Name);

                // Populate UserName and CityName
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

        // GET: ProfessionalController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProfessionalController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfessionalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfessionalController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProfessionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
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
            return View();
        }

        // POST: ProfessionalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
