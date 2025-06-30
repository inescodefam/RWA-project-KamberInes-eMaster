using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
namespace WebAPI.Controllers
{
    [Route("api/servicetype")]
    [ApiController]
    [Authorize]
    public class ServiceTypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceType _serviceTypeService;

        public ServiceTypeController(IMapper mapper, IServiceType serviceType)
        {
            _mapper = mapper;
            _serviceTypeService = serviceType;
        }

        [HttpGet]
        public ActionResult<List<ServiceTypeApiDto>> Get(int count, int start = 0)
        {
            try
            {
                var serviceTypesDto = _serviceTypeService.GetServiceTypes(count, start);
                return Ok(serviceTypesDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service types.");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateServiceTypeApiDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.ServiceTypeName))
            {
                return BadRequest("Service type name is required.");
            }

            try
            {
                var serviceType = _mapper.Map<ServiceTypeDto>(dto);
                var entity = _serviceTypeService.CreateServiceType(serviceType);

                return CreatedAtAction(nameof(Get), new { id = entity.IdserviceType }, dto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the service type.");
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody] ServiceTypeApiDto dto)
        {
            if (dto == null || dto.IdserviceType <= 0 || string.IsNullOrWhiteSpace(dto.ServiceTypeName))
            {
                return BadRequest("Invalid service type data.");
            }
            try
            {
                ServiceTypeDto serviceDto = _mapper.Map<ServiceTypeDto>(dto);
                var updatedServiceType = _serviceTypeService.UpdateServiceType(serviceDto);
                return Ok(updatedServiceType);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the service type.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _serviceTypeService.DeleteServiceType(id);
                if (!result)
                {
                    return NotFound();
                }
                return NotFound($"Service type with ID {id} not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the service type.");
            }
        }

    }
}
