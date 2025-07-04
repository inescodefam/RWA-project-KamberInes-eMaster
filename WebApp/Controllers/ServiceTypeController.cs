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
        public IActionResult Index(int count, int start)
        {
            try
            {
                var serviceType = _serviceTypeService.GetServiceTypes(count, start);

                ServiceTypeIndexVm serviceTypeIndexVm = new ServiceTypeIndexVm
                {
                    ServiceTypes = serviceType
                };

                return View(serviceTypeIndexVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
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
                    var response = _serviceTypeService.CreateServiceType(model);
                    return RedirectToAction("Index", "ServiceType");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ServiceTypeName", ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int serviceTypeId, string name)
        {
            if (!ModelState.IsValid || serviceTypeId <= 0)
                return RedirectToAction("Index");
            try
            {
                ServiceTypeVM model = new ServiceTypeVM
                {
                    IdserviceType = serviceTypeId,
                    ServiceTypeName = name
                };

                var response = _serviceTypeService.UpdateServiceType(model);
                if (response == null)
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

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid || id <= 0)
                return RedirectToAction("Index");

            try
            {
                var response = _serviceTypeService.DeleteServiceType(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return RedirectToAction("Index");
            }
        }
    }
}
