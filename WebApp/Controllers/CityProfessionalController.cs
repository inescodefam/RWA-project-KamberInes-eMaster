using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class CityProfessionalController : Controller
    {
        private readonly ICityProfessionalService _cityProfessionalService;

        public CityProfessionalController(ICityProfessionalService cityProfessionalService)
        {
            _cityProfessionalService = cityProfessionalService;
        }

        [HttpGet]
        public IActionResult Index(int count, int start)
        {
            var result = _cityProfessionalService.GetCityProfessionals(count, start);
            return View(result);
        }

        [HttpGet]
        public IActionResult GetProfessionals(int cityId)
        {
            var response = _cityProfessionalService.GetProfessionalsByCity(cityId);
            var result = new CityProfessionalsVm
            {
                Professionals = response.Select(p => new ProfessionalDataVM
                {
                    IdProfessional = p.IdProfessional,
                    UserId = p.UserId,
                    UserName = p.UserName,
                    Email = p.Email,
                    Phone = p.Phone,
                    FirstName = p.FirstName,
                    LastName = p.LastName
                }).ToList(),
                CityId = cityId
            };

            return View(result);
        }

        [HttpGet]
        public IActionResult GetCities(int professionalId)
        {
            var result = _cityProfessionalService.GetCitysByProfessional(professionalId);
            return View();
        }

        [HttpGet]
        public IActionResult AddCityProfessional()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCityProfessional(CityProfessionalVM model)
        {
            if (ModelState.IsValid)
            {
                var result = _cityProfessionalService.AddCityProfessional(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateCityProfessional(int id)
        {
            var model = _cityProfessionalService.GetCityProfessionals(0, 0).FirstOrDefault(x => x.IdProfessionalCity == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateCityProfessional(int id, CityProfessionalDataVM model)
        {
            if (ModelState.IsValid)
            {
                var result = _cityProfessionalService.UpdateCityProfessional(id, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateCitiesByProfessional(int professionalId, List<int> citiesIds)
        {
            if (ModelState.IsValid)
            {
                var result = _cityProfessionalService.UpdateCitiesByProfessional(professionalId, citiesIds);
                return RedirectToAction("Index");
            }
            return BadRequest("Invalid data");
        }

        [HttpPost]
        public IActionResult UpdateProfessionalsByCity(int cityId, List<int> professionalsIds)
        {
            if (ModelState.IsValid)
            {
                var result = _cityProfessionalService.UpdateProfessionalsByCity(cityId, professionalsIds);
                return RedirectToAction("Index", "City");
            }
            return BadRequest("Invalid data");
        }

        [HttpPost]
        public IActionResult DeleteCityProfessional(int idProfessionalCity)
        {
            if (_cityProfessionalService.DeleteCityProfessional(idProfessionalCity))
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Error deleting city professional");
        }

        [HttpPost]
        public IActionResult DeleteCitiesForProfessional(int professionalId)
        {
            if (_cityProfessionalService.DeleteCitiesForProfessional(professionalId))
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Error deleting cities for professional");
        }

        [HttpPost]
        public IActionResult DeleteProfessionalsForCity(int id)
        {
            if (_cityProfessionalService.DeleteProfessionalsForCity(id))
            {
                return RedirectToAction("Create", "Professional");
            }
            return BadRequest("Error deleting professionals for city");
        }
    }
}

