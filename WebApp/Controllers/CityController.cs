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
        public IActionResult Index(string searchTerm, int page, int pageSize)
        {
            var response = _cityService.GetCities(searchTerm, page, pageSize);
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

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CityCreateVM());
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
                    return View(model);
                }
                return RedirectToAction("Index");

            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return View(model);
            }
        }

    }
}
