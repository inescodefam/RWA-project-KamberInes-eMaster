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
        private readonly ICityService _cityService;
        private readonly IServiceType _serviceType;


        public ServiceController(
            IMapper mapper,
            IServiceService serviceApiService,
            ICityService cityService,
            IServiceType serviceType)
        {

            _serviceService = serviceApiService;
            _cityService = cityService;
            _serviceType = serviceType;
        }

        [HttpGet]
        public IActionResult Search()
        {
            var vm = new ServiceSearchVM
            {
                Cities = _cityService.GetAllCities(),
                ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
                Services = _serviceService.GetServiceIndex(50, 0)
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Search(string serviceTypeName, int count = 50, int start = 0)
        {// add city id...
            if (string.IsNullOrEmpty(serviceTypeName))
            {
                serviceTypeName = string.Empty;
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
