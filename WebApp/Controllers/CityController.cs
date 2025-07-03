using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models;


namespace WebApp.Controllers
{
    [Authorize]
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
        public IActionResult Index(string searchTerm, int pageSize, int page)
        {
            var response = _cityService.GetCities(searchTerm, pageSize, page);
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
        public IActionResult Create(CityCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = _cityService.CreateCity(model.Name);

                if (response == null)
                {
                    ModelState.AddModelError("", "Failed to create city. Please try again.");
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");

            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return RedirectToAction("Index");
            }
        }

    }
}
