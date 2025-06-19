using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Models;
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

        public ServiceTypeController(EProfessionalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<ServiceTypeDto>> GetServiceTypes(int count, int start = 0)
        {
            try
            {
                var serviceTypes = _context.ServiceTypes
                    .Skip(start * count)
                    .Take(count)
                    .Select(st => new ServiceTypeDto
                    {
                        IdserviceType = st.IdserviceType,
                        ServiceTypeName = st.ServiceTypeName
                    })
                    .ToList();

                var serviceTypesDto = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);
                return Ok(serviceTypesDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service types.");
            }
        }

        [HttpPost]
        public IActionResult CreateServiceType([FromBody] ServiceTypeDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.ServiceTypeName))
            {
                return BadRequest("Service type name is required.");
            }

            try
            {

                var entity = new ServiceType
                {
                    ServiceTypeName = dto.ServiceTypeName
                };

                _context.ServiceTypes.Add(entity);
                _context.SaveChanges();

                dto.IdserviceType = entity.IdserviceType;

                return CreatedAtAction(nameof(GetServiceTypes), new { id = entity.IdserviceType }, dto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the service type.");
            }
        }

    }
}
