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
        private readonly EMasterDbContext _context;
        private readonly IMapper _mapper;
        public ProfessionalsController(IConfiguration configuration, EMasterDbContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ProfessionalsController>
        [HttpGet]
        public ActionResult<IEnumerable<Professional>> Get()
        {
            try
            {
                var result = _context.Professionals.ToList();
                if (result == null || result.Count == 0)
                {
                    return NotFound("No professionals found");
                }

                var professionalDto = _mapper.Map<List<ProfessionalDto>>(result);
                return Ok(professionalDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProfessionalsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProfessionalsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
        }
    }
}
