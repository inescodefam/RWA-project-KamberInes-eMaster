using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Context;

namespace WebAPI.Controllers
{
    [Route("api/servicetype")]
    [ApiController]
    [Authorize]
    public class ServiceTypeController : Controller
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;
        private readonly IServiceType _serviceTypeService;

        public ServiceTypeController(EProfessionalContext context, IMapper mapper, IServiceType serviceType)
        {
            _context = context;
            _mapper = mapper;
            _serviceTypeService = serviceType;
        }

        [HttpGet]
        public ActionResult<List<ServiceTypeDto>> Get(int count, int start = 0)
        {
            try
            {
                var serviceTypesDto = _serviceTypeService.GetServiceTypes(count, start).Result;
                return Ok(serviceTypesDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service types.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceTypeDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.ServiceTypeName))
            {
                return BadRequest("Service type name is required.");
            }

            try
            {

                var entity = await _serviceTypeService.CreateServiceType(dto);

                return CreatedAtAction(nameof(Get), new { id = entity.IdserviceType }, dto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the service type.");
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ServiceTypeDto dto)
        {
            if (dto == null || dto.IdserviceType <= 0 || string.IsNullOrWhiteSpace(dto.ServiceTypeName))
            {
                return BadRequest("Invalid service type data.");
            }
            try
            {
                var updatedServiceType = await _serviceTypeService.UpdateServiceType(dto);
                return Ok(updatedServiceType);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the service type.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _serviceTypeService.DeleteServiceType(id);
                if (result)
                {
                    return NoContent();
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
