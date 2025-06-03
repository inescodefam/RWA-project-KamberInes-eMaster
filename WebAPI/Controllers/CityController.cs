using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/city")]
    [ApiController]
    [Authorize]
    public class CityController : Controller
    {
        private readonly EProfessionalContext _context;

        public CityController(EProfessionalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<CityDto>> GetCities(int count, int start = 0)
        {

            try
            {
                return Ok(_context.Cities.Skip(start * count).Take(count));
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }
    }
}
