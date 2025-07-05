using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProfessionalController : Controller
    {
        private readonly IProfessionalService _professionalService;
        private readonly ICityProfessionalService _cityProfessionalService;

        public ProfessionalController(IProfessionalService professionalService, ICityProfessionalService cityProfessionalService)
        {
            _professionalService = professionalService;
            _cityProfessionalService = cityProfessionalService;
        }

        // GET: ProfessionalApiController
        [HttpGet]
        public IActionResult Index(int pageSize, int page)
        {
            var model = _professionalService.GetProfessionals(pageSize, page);

            if (model == null)
            {
                ModelState.AddModelError("", "No professionals found.");
                return View(new ProfessionalIndexVM
                {
                    Professionals = new List<ProfessionalVM>()
                });
            }

            return View(model);
        }

        // GET: ProfessionalApiController/Search
        [HttpGet]
        public IActionResult Search(string username, string city, int pageSize, int page, bool partial = false)
        {
            var response = _professionalService.Search(username, city, pageSize, page);

            if (partial) return PartialView("_ProfessionalTablePartial", response);

            return Json(response);
        }

        // GET: ProfessionalApiController/AddProfessional
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View(_professionalService.GetProfessionals(50, 0));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] ProfessionalBaseVm professionalVM)
        {

            if (professionalVM == null)
            {
                ModelState.AddModelError("", "Invalid data submitted.");
                return BadRequest("Invalid data submitted.");
            }

            try
            {

                var response = _professionalService.CreateProfessional(professionalVM);
                return response ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException(ex.Message);
            }


        }

        public ActionResult Details(int id)
        {
            var professional = _professionalService.GetSingleProfessional(id);
            return View(professional);
        }



        // GET: ProfessionalApiController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var professional = _professionalService.GetSingleProfessionalIndexVm(id);

            return View(professional);
        }

        // POST: ProfessionalApiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([FromForm] ProfessionalCityEditVM professionalEditVm)
        {
            try
            {
                //var response = _professionalService.UpdateProfessional(professionalEditVm);

                var response = _cityProfessionalService.UpdateCitiesByProfessional(professionalEditVm.IdProfessional, professionalEditVm.CityIds);
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid data submitted.");
                    var model = _professionalService.GetSingleProfessionalIndexVm(professionalEditVm.IdProfessional);
                    return View(model);
                }

                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // delete: ProfessionalApiController/Delete/5
        [Route("Professional/Delete/{id}")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = _professionalService.DeleteProfessional(id);
                if (!response)
                    return NoContent();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
