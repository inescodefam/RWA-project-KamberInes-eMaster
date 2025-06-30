using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IServiceService _servicesService;
        private readonly IMapper _mapper;

        public ServiceController(IServiceService servicesService, IMapper mapper)
        {
            _servicesService = servicesService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<ServiceApiDto>> GetServices(int count, int start = 0)
        {
            try
            {
                var services = _servicesService.GetServicesCount(count, start);
                return Ok(services);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the services.");
            }
        }

        [HttpGet("type/{type}")]
        public ActionResult<ServiceApiDto> GetServiceByServiceType(string type, int count)
        {
            try
            {
                return Ok(_servicesService.GetServiceByServiceType(type, count));
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service.");
            }
        }

        [HttpGet("id/{id}")]
        public ActionResult<ServiceApiDto> GetServiceByServiceId(int id)
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
        public ActionResult<ServiceApiDto> CreateServiceFromDto(CreateServiceApiDto serviceDto)
        {
            try
            {
                if (serviceDto == null)
                {
                    return BadRequest("Service cannot be null.");
                }

                var service = _mapper.Map<ServiceDto>(serviceDto);
                _servicesService.CreateService(service);
                return CreatedAtAction(nameof(GetServiceByServiceType), new { type = serviceDto.ServiceTypeId }, serviceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateService(int id, ServiceApiDto serviceApiDto)
        {
            ServiceDto exists = _servicesService.GetServiceByServiceId(serviceApiDto.IdService);
            if (exists == null)
            {
                return BadRequest("Service does not exist.");
            }
            try
            {
                var serviceDto = _mapper.Map<ServiceDto>(serviceApiDto);
                _servicesService.UpdateService(serviceDto);
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
