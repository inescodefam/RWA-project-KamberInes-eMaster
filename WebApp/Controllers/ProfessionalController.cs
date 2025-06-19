using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
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
        private readonly ProfessionalViewModelService _viewModelService;

        public ProfessionalController(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService, ProfessionalViewModelService viewModelService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
            _viewModelService = viewModelService;
        }

        // GET: ProfessionalController
        [HttpGet]
        public async Task<IActionResult> Index(int count = 50, int start = 0)
        {
            var model = await _viewModelService.GetProfessionalIndexVM(count, start);

            return View(model);
        }

        // GET: ProfessionalController/AddProfessional
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _viewModelService.GetProfessionalIndexVM();

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var professionals = _apiFetchService.FetchList<ProfessionalDto, ProfessionalVM>($"api/professional/{id}", this);
            return View(professionals);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfessionalVM model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Invalid data submitted.");
                return BadRequest("Invalid data submitted.");
            }

            var newProfessional = _mapper.Map<ProfessionalDto>(model);
            var response = await _httpClient.PostAsJsonAsync("api/professional", newProfessional);

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest();

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
        public ActionResult Delete() => View();

        // delete: ProfessionalController/Delete/5
        [Route("Professional/Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/professional/{id}");
                if (response.IsSuccessStatusCode)
                    return NoContent();

                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
