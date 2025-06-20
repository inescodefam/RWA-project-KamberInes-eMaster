using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/professional")]
    [Authorize]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly LogService _loggingService;

        private readonly IProfessionalService _professionalService;
        public ProfessionalController(LogService logService, IProfessionalService professionalService)
        {
            _loggingService = logService;
            _professionalService = professionalService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProfessionalDto>> GetAllProfessionals(int count, int start = 0)
        {
            try
            {
                var professionals = _professionalService.GetProfessionals(count, start);
                return Ok(professionals);
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error retrieving professionals: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProfessionalsController>/5
        [HttpGet("{id}")]
        public ActionResult<ProfessionalDto> GetSingleProfessionalById(int id)
        {
            try
            {
                var professionalDto = _professionalService.GetSingleProfessional(id);
                return Ok(professionalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // get professional by name name in user
        [HttpGet("search")]
        public ActionResult<IEnumerable<ProfessionalDto>> Search(
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] int count,
            [FromQuery] int start = 0
            )
        {
            try
            {
                var professionalDtos = _professionalService.SearchProfessionals(name, city, count, start);
                return Ok(professionalDtos);

            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error retrieving professionals by name '': {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ProfessionalsController>
        [HttpPost]
        public IActionResult Post([FromBody] ProfessionalDto professionalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _professionalService.CreateProfessional(professionalDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error adding professional: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProfessionalsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProfessionalDto professionalDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            try
            {
                var response = _professionalService.UpdateProfessional(id, professionalDto);
                if (!response) NotFound();

                Ok("Professional updated successfully");
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error updating professional with ID {id}: {ex.Message}", "error");
                StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ProfessionalsController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

            try
            {
                var response = _professionalService.DeleteProfessional(id);
                if (!response)
                {
                    NotFound();
                }
                Ok("Professional deleted successfully");
            }
            catch (Exception ex)
            {
                StatusCode(500, ex.Message);
            }
        }
    }
}
