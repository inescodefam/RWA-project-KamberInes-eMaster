using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EMasterContext _context;
        private readonly IMapper _mapper;
        public ProfessionalsController(IConfiguration configuration, EMasterContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ProfessionalsController>
        [HttpGet]
        public ActionResult<IEnumerable<Professional>> GetAllProfessionals()
        {
            try
            {
                var professionals = _context.Professionals.ToList();
                if (professionals == null || professionals.Count == 0)
                {
                    return NotFound("No professionals found");
                }

                var professionalDto = _mapper.Map<List<ProfessionalDto>>(professionals);
                return Ok(professionalDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProfessionalsController>/5
        [HttpGet("{id}")]
        public ActionResult<ProfessionalDto> GetSingleProfessional(int id)
        {
            var professional = _context.Professionals.Find(id);
            if (professional == null)
                return NotFound();

            var professionalDto = _mapper.Map<ProfessionalDto>(professional);
            return Ok(professionalDto);
        }

        // POST api/<ProfessionalsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var professional = _mapper.Map<Professional>(value);
            try
            {
                _context.Professionals.Add(professional);
                _context.SaveChanges();
                Ok("Professional added successfully");
            }
            catch (Exception ex)
            {
                StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProfessionalsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProfessionalsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var professional = _context.Professionals.Find(id);
            if (professional == null)
            {
                NotFound();
                return;
            }
            try
            {
                _context.Professionals.Remove(professional);
                _context.SaveChanges();
                Ok("Professional deleted successfully");
            }
            catch (Exception ex)
            {
                StatusCode(500, ex.Message);
            }
        }
    }
}
