using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]

    public class ServiceController : Controller
    {

        private readonly IServiceService _serviceService;


        public ServiceController(IMapper mapper, IServiceService serviceApiService)
        {

            _serviceService = serviceApiService;
        }

        [HttpGet]
        public IActionResult Index(int count, int start)
        {
            var response = _serviceService.GetServiceIndex(count, start);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult Search(string serviceTypeName, int count, int start)
        {
            if (string.IsNullOrEmpty(serviceTypeName))
            {
                ModelState.AddModelError("serviceTypeName", "Service type name cannot be empty.");
                return View(new ServiceSearchVM());
            }

            var vm = new ServiceSearchVM
            {
                Services = _serviceService.Search(serviceTypeName, count, start)
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var response = _serviceService.GetServiceByID(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new ServiceCreateVM { };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(ServiceCreateVM vm)
        {

            return View(vm);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vm = _serviceService.GetServiceByID(id);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(ServiceEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            _serviceService.EditService(vm);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {
            var response = _serviceService.DeleteService(id);

            return RedirectToAction("Search");
        }

        public IActionResult YourServices()
        {
            return View();
        }

    }

}
