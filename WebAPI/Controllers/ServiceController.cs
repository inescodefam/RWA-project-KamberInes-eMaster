using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly ServicesService _servicesService;

        public ServiceController(ServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        public ActionResult<List<Service>> GetServices(int count, int start = 0)
        {
            try
            {
                var services = _servicesService.GetServices(count, start);
                return Ok(services);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the services.");
            }
        }
        [HttpGet("type/{type}")]
        public ActionResult<Service> GetServiceByServiceType(string type)
        {
            try
            {
                return Ok(_servicesService.GetServiceByServiceType(type));
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service.");
            }
        }

        [HttpGet("id/{id}")]
        public ActionResult<ServiceDto> GetServiceByServiceType(int id)
        {
            try
            {
                return Ok(_servicesService.GetServiceByServiceId(id));
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service.");
            }
        }

        [HttpPost]
        public ActionResult<Service> CreateServiceFromDto(ServiceDto serviceDto)
        {
            try
            {
                if (serviceDto == null)
                {
                    return BadRequest("Service cannot be null.");
                }
                _servicesService.CreateService(serviceDto);
                return CreatedAtAction(nameof(GetServiceByServiceType), new { type = serviceDto.ServiceTypeId }, serviceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateService(int id, ServiceDto service)
        {
            if (id != service.IdService)
            {
                return BadRequest("Service ID mismatch.");
            }
            try
            {

                _servicesService.UpdateService(id, service);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteService(int id)
        {
            try
            {
                _servicesService.DeleteService(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
