using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]

    public class ServiceTypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceType _serviceTypeService;

        public ServiceTypeController(IMapper mapper, ApiService apiFetchService, IServiceType serviceType)
        {
            _mapper = mapper;
            _serviceTypeService = serviceType;
        }

        [HttpGet]
        public IActionResult CreateServiceType() => View();

        [HttpPost]
        public IActionResult CreateServiceType(ServiceTypeVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = _serviceTypeService.UpdateServiceType(model);
                    return RedirectToAction("Search", "Service");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ServiceTypeName", ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
