using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;
        private readonly LogService _loggingService;
        public ProfessionalController(IConfiguration configuration, EProfessionalContext context, IMapper mapper, LogService logService)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _loggingService = logService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProfessionalDto>> GetAllProfessionals(int count, int start = 0)
        {
            try
            {
                var professionals = _context.Professionals.Skip(start * count).Take(count).ToList();

                if (professionals == null || professionals.Count == 0)
                {
                    return NotFound("No professionals found");
                }

                var professionalDto = _mapper.Map<List<ProfessionalDto>>(professionals);
                _loggingService.Log($"Retrieved {professionalDto.Count} professionals from the database.", "info");
                return Ok(professionalDto);
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error retrieving professionals: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProfessionalsController>/5
        [HttpGet("{id}")]
        public ActionResult<ProfessionalDto> GetSingleProfessional(int id)
        {
            var professional = _context.Professionals.Find(id);
            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found.", "warning");
                return NotFound();
            }

            var professionalDto = _mapper.Map<ProfessionalDto>(professional);
            _loggingService.Log($"Retrieved professional with ID {id} from the database.", "info");
            return Ok(professionalDto);
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
                var query = _context.Professionals.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.User.FirstName == name || p.User.LastName == name);
                }
                if (!string.IsNullOrEmpty(city))
                {
                    query = query.Where(p => p.City.City1.Contains(city));
                }
                var professionals = query.Skip(start * count).Take(count).ToList();
                if (professionals == null || professionals.Count == 0)
                {
                    _loggingService.Log($"No professionals found for name '{name}' and city '{city}'.", "info");
                    return NotFound("No professionals found");
                }
                var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);
                _loggingService.Log($"Retrieved {professionalDtos.Count} professionals for name '{name}' and city '{city}'.", "info");
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
        public void Post([FromBody] ProfessionalDto professionalDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var professional = new Professional
            {
                UserId = professionalDto.UserId
            };

            try
            {
                _context.Professionals.Add(professional);
                _context.SaveChanges();
                _loggingService.Log($"Professional with ID {professional.IdProfessional} added successfully.", "info");
                Ok("Professional added successfully");
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error adding professional: {ex.Message}", "error");
                StatusCode(500, ex.Message);
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
            var professional = _context.Professionals.Find(id);
            if (professional == null)
            {
                NotFound();
                _loggingService.Log($"Professional with ID {id} not found for update.", "warning");
                return;
            }
            try
            {
                professional = _mapper.Map(professionalDto, professional);
                _context.Professionals.Update(professional);
                _context.SaveChanges();
                _loggingService.Log($"Professional with ID {id} updated successfully.", "info");
                Ok("Professional updated successfully");
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error updating professional with ID {id}: {ex.Message}", "error");
                StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ProfessionalsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var professional = _context.Professionals.Find(id);
            if (professional == null)
            {
                NotFound();
                _loggingService.Log($"Professional with ID {id} not found for deletion.", "warning");
                return;
            }
            try
            {
                _context.Professionals.Remove(professional);
                _context.SaveChanges();
                _loggingService.Log($"Professional with ID {id} deleted successfully.", "info");
                Ok("Professional deleted successfully");
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error deleting professional with ID {id}: {ex.Message}", "error");
                StatusCode(500, ex.Message);
            }
        }
    }
}
