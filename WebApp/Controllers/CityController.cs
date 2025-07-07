using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models;


namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public IActionResult Index(string searchTerm, int pageSize, int page, bool partial)
        {
            var response = _cityService.GetCities(searchTerm, pageSize, page);
            var total = _cityService.GetCityCount(searchTerm);
            if (pageSize == 0) pageSize = 10;

            var model = new CityIndexVM
            {
                Cities = _mapper.Map<List<CityVM>>(response),
                SearchTerm = searchTerm,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            if (partial)
                return PartialView("_CityTablePartial", model);

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CityCreateVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = _cityService.CreateCity(model.Name);
                    return RedirectToAction("Index");

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid || id <= 0)
                return RedirectToAction("Index");

            try
            {
                var response = _cityService.DeleteCity(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Update(int cityId, string name)
        {
            try
            {
                var response = _cityService.UpdateCity(cityId, name);

                if (!response)
                {
                    return Json(new { success = false, message = "Failed to update city" });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
