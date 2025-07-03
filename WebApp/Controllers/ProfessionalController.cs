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
        public IActionResult Index(int count = 50, int start = 0)
        {
            var model = _professionalService.GetProfessionals(count, start);

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
        public IActionResult Search(string username, string city, int count, int start)
        {
            var response = _professionalService.Search(username, city, count, start);

            return View(response);
        }

        // GET: ProfessionalApiController/AddProfessional
        [HttpGet]
        public IActionResult Create() => View(_professionalService.GetProfessionals(50, 0));

        [HttpPost]
        public IActionResult Create([FromBody] ProfessionalBaseVm professionalVM)
        {

            if (professionalVM == null)
            {
                ModelState.AddModelError("", "Invalid data submitted.");
                return BadRequest("Invalid data submitted.");
            }

            var response = _professionalService.CreateProfessional(professionalVM);

            return response ? Ok() : BadRequest();

        }

        public ActionResult Details(int id)
        {
            var professional = _professionalService.GetSingleProfessional(id);
            return View(professional);
        }



        // GET: ProfessionalApiController/Edit/5
        public ActionResult Edit(int id)
        {
            var professional = _professionalService.GetSingleProfessional(id);

            return View(professional);
        }

        // POST: ProfessionalApiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] ProfessionalDataVM professionalVm)
        {
            try
            {
                var response = _professionalService.UpdateProfessional(professionalVm);
                if (!response)
                {
                    ModelState.AddModelError("", "Failed to update professional.");

                    return View(response);
                }
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }


        // GET: ProfessionalApiController/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete() => View();

        // delete: ProfessionalApiController/Delete/5
        [Route("Professional/Delete/{id}")]
        [HttpDelete]
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
