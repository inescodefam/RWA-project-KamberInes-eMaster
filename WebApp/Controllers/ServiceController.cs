﻿using AutoMapper;
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
        private readonly IProfessionalService _professionalService;


        public ServiceController(
            IMapper mapper,
            IServiceService serviceApiService,
            ICityService cityService,
            IServiceType serviceType,
            IProfessionalService professionalService)
        {

            _serviceService = serviceApiService;
            _cityService = cityService;
            _serviceType = serviceType;
            _professionalService = professionalService;
        }

        [HttpGet]
        public IActionResult Search()
        {
            int pageSize = 10;
            int page = 0;

            var vm = new ServiceSearchVM
            {
                Cities = _cityService.GetAllCities(),
                ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
                Services = _serviceService.GetServiceIndex(pageSize, page),
                PageSize = pageSize,
                Page = page,
                TotalCount = _serviceService.GetServiceCount()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Search(string SelectedServiceTypeName, int SelectedCityId, int pageSize, int page, bool partial)
        {
            int totalCount;
            if (string.IsNullOrEmpty(SelectedServiceTypeName))
            {
                SelectedServiceTypeName = string.Empty;
                totalCount = _serviceService.GetServiceCount();
            }
            else
            {
                totalCount = _serviceService.GetServiceCountForServiceTypeName(SelectedServiceTypeName);
            }

            var services = _serviceService.Search(SelectedServiceTypeName, SelectedCityId, pageSize, page);
            var totalServices = services.Count;
            if (pageSize > totalServices && page == 0) totalCount = totalServices;

            var vm = new ServiceSearchVM
            {
                Cities = _cityService.GetAllCities(),
                ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
                Services = services,
                PageSize = pageSize,
                Page = page,
                TotalCount = totalCount
            };

            if (partial)
            {
                return PartialView("_ServiceTablePartial", vm);
            }

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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var vm = new ServiceCreateVM
            {
                ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
                Cities = _cityService.GetAllCities(),
                Professionals = _professionalService.GetProfessionals(1000).Professionals
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateServiceResultVM vm)
        {
            if (!ModelState.IsValid)
            {
                if (vm.SelectedCitiesIds.Count == 0)
                {
                    ModelState.AddModelError("SelectedCitiesIds", "At least one city must be selected.");
                }
                vm = new ServiceCreateVM
                {
                    ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
                    Cities = _cityService.GetAllCities(),
                    Professionals = _professionalService.GetProfessionals(1000).Professionals
                };
                ModelState.AddModelError("", "Please correct the errors in the form.");
                return View(vm);
            }

            var result = _serviceService.CreateService(vm);
            if (!result)
            {
                vm = new ServiceCreateVM
                {
                    ServiceTypes = _serviceType.GetServiceTypes(1000, 0),
                    Cities = _cityService.GetAllCities(),
                    Professionals = _professionalService.GetProfessionals(1000).Professionals
                };
                ModelState.AddModelError("", "Please correct the errors in the form.");
                return View(vm);
            }

            return Redirect("Search");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var vm = _serviceService.GetServiceByIdEditVm(id);
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ServiceEditResultVM vm)
        {


            if (!ModelState.IsValid)
            {
                var model = _serviceService.GetServiceByIdEditVm(vm.IdService);
                return View(model);
            }

            _serviceService.EditService(vm);

            return RedirectToAction(nameof(Search));
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
