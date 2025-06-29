using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/professional")]
    [Authorize]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly LogService _loggingService;
        private readonly IMapper _mapper;
        private readonly IProfessionalService _professionalService;
        public ProfessionalController(
            LogService logService,
            IProfessionalService professionalService,
            IMapper mapper)
        {
            _loggingService = logService;
            _professionalService = professionalService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProfessionalApiDto>> GetAllProfessionals(int count, int start = 0)
        {
            if (count <= 0 || start < 0)
                return BadRequest("Invalid paging parameters");
            try
            {
                var professionals = _professionalService.GetProfessionals(count, start);

                if (professionals == null || !professionals.Any())
                {
                    _loggingService.CreateLog("No professionals found in the database.", "info");
                    return NoContent();
                }

                _loggingService.CreateLog($"Retrieved {professionals.Count} professionals from the database.", "info");
                return Ok(professionals);
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error retrieving professionals: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProfessionalsController>/5
        [HttpGet("{id}")]
        public ActionResult<ProfessionalApiDto> GetSingleProfessionalById(int id)
        {
            try
            {
                var professionalDto = _professionalService.GetSingleProfessional(id);
                _loggingService.CreateLog($"Retrieved professional with ID {id}.", "info");
                return Ok(professionalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                _loggingService.CreateLog($"Error retrieving professional with ID {id}: {ex.Message}", "error");
            }
        }

        // get professional by name name in user
        [HttpGet("search")]
        public ActionResult<IEnumerable<ProfessionalApiDto>> Search(
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] int count,
            [FromQuery] int start = 0
            )
        {
            try
            {
                var professionalDtos = _professionalService.Search(name, city, count, start);
                if (professionalDtos == null || !professionalDtos.Any())
                {
                    _loggingService.CreateLog("No professionals found matching the search criteria.", "info");
                    return NoContent();
                }
                _loggingService.CreateLog($"Found {professionalDtos.Count} professionals matching the search criteria.", "info");
                return Ok(professionalDtos);

            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error retrieving professionals by name '': {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ProfessionalsController>
        [HttpPost]
        public IActionResult Post([FromBody] ProfessionalApiDto professionalApiDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var professionalDto = _mapper.Map<ProfessionalDto>(professionalApiDto);
                var response = _professionalService.CreateProfessional(professionalDto);
                if (!response)
                {
                    _loggingService.CreateLog("Failed to create professional.", "error");
                    return BadRequest("Failed to create professional.");
                }
                _loggingService.CreateLog("Professional created successfully.", "info");
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error adding professional: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProfessionalsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProfessionalApiDto professionalApiDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var professionalDto = _mapper.Map<ProfessionalDto>(professionalApiDto);
                var response = _professionalService.UpdateProfessional(id, professionalDto);
                if (!response)
                {
                    _loggingService.CreateLog($"Failed to update professional with ID {id}.", "error");
                    return NotFound();
                }

                _loggingService.CreateLog($"Professional with ID {id} updated successfully.", "info");
                return Ok("Professional updated successfully");
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error updating professional with ID {id}: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ProfessionalsController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = _professionalService.DeleteProfessional(id);
                if (!response)
                {
                    _loggingService.CreateLog($"Failed to delete professional with ID {id}.", "error");
                    return NotFound();
                }
                _loggingService.CreateLog($"Professional with ID {id} deleted successfully.", "info");
                return Ok("Professional deleted successfully");
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error deleting professional with ID {id}: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
