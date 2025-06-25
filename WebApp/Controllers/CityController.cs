using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.Services;
using WebApp.Models;


namespace WebApp.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        public CityController(IMapper mapper, ICityService cityService)
        {
            _mapper = mapper;
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm, int page = 1, int pageSize = 10)
        {
            var response = await _cityService.GetCitiesAsync(searchTerm, page, pageSize);
            var model = new CityIndexVM
            {
                Cities = _mapper.Map<List<CityVM>>(response),
                SearchTerm = searchTerm,
                Page = page,
                PageSize = pageSize,
                TotalCount = response.Count
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCityAsync(CityIndexVM model)
        {

            var response = await _cityService.CreateCityAsync(model.NewCityName);
            if (response != null)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to add city. Please try again later.");
            return RedirectToAction("Index");
        }

    }
}
