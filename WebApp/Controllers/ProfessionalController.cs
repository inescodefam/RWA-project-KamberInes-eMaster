using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ProfessionalController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;
        private readonly ProfessionalViewModelService _viewModelService;
        private readonly IProfessionalService _professionalService;

        public ProfessionalController(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService, ProfessionalViewModelService viewModelService, IProfessionalService professionalService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
            _viewModelService = viewModelService;
            _professionalService = professionalService;
        }

        // GET: ProfessionalController
        [HttpGet]
        public async Task<IActionResult> Index(int count = 50, int start = 0)
        {
            var model = await _professionalService.GetProfessionals(count, start);
            List<ProfessionalVM> professionalVMs = _mapper.Map<List<ProfessionalVM>>(model);

            if (professionalVMs == null || professionalVMs.Count == 0)
            {
                ModelState.AddModelError("", "No professionals found.");
                return View(new ProfessionalIndexVM
                {
                    Professionals = new List<ProfessionalVM>()
                });
            }
            var professionalIndexModel = await _viewModelService.GetProfessionalIndexVM(professionalVMs, count);

            return View(professionalIndexModel);
        }

        // GET: ProfessionalController/AddProfessional
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var professionals = await _professionalService.GetProfessionals(1000, 0);
            int count = professionals.Count;
            var professionalVms = _mapper.Map<List<ProfessionalVM>>(professionals);
            var model = await _viewModelService.GetProfessionalIndexVM(professionalVms, count);

            return View(model);
        }

        // GET: ProfessionalController/Search
        [HttpGet]
        public async Task<IActionResult> Search(string username, string? city, int? count, int? start)
        {
            var response = await _professionalService.Search(username, city, 1000, 0);
            List<ProfessionalVM> professionalVms = _mapper.Map<List<ProfessionalVM>>(response);
            ProfessionalIndexVM model;

            if (professionalVms == null || professionalVms.Count == 0)
            {
                ModelState.AddModelError("", "No professionals found.");
                model = new ProfessionalIndexVM
                {
                    Professionals = new List<ProfessionalVM>()
                };
            }
            else
            {

                model = await _viewModelService.GetProfessionalIndexVM(professionalVms, professionalVms.Count);
            }


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { model.Professionals });
            }
            else
            {
                return View("Index", model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfessionalVM professionalVM)
        {

            var professionalDto = _mapper.Map<ProfessionalDto>(professionalVM);

            if (professionalDto == null)
            {
                ModelState.AddModelError("", "Invalid data submitted.");
                return BadRequest("Invalid data submitted.");
            }

            var response = await _professionalService.CreateProfessional(professionalDto);

            if (response)
                return Ok();

            return BadRequest();

        }

        public ActionResult Details(int id)
        {
            var professional = _professionalService.GetSingleProfessional(id);
            return View(professional);
        }



        // GET: ProfessionalController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var professional = await _professionalService.GetSingleProfessional(id);
            ProfessionalVM professionalVm = _mapper.Map<ProfessionalVM>(professional);

            var professionalVmList = new List<ProfessionalVM> { professionalVm };

            ProfessionalIndexVM professionalIndexVM = await _viewModelService.GetProfessionalIndexVM(professionalVmList, 1);

            return View(professionalIndexVM);
        }

        // POST: ProfessionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProfessionalDto professionalDto)
        {
            try
            {
                var response = _professionalService.UpdateProfessional(id, professionalDto);
                if (!response)
                {
                    ModelState.AddModelError("", "Failed to update professional.");
                    return View(professionalDto);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfessionalController/Delete/5
        public ActionResult Delete() => View();

        // delete: ProfessionalController/Delete/5
        [Route("Professional/Delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = _professionalService.DeleteProfessional(id);
                if (response)
                    return NoContent();

                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
