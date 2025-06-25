using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Models;

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

    }
}
