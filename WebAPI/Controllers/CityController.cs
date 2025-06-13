using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Models;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/city")]
    [ApiController]
    [Authorize]
    public class CityController : Controller
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;

        public CityController(EProfessionalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        [HttpPost]
        public ActionResult<CityDto> CreateCity([FromBody] string cityName)
        {
            if (cityName == null)
            {
                return BadRequest("City data is required.");
            }
            try
            {
                if (_context.Cities.Any(x => x.City1 == cityName))
                {
                    return Conflict("City already exists.");
                }

                var city = new City
                {
                    City1 = cityName,

                };
                _context.Cities.Add(city);
                _context.SaveChanges();

                City c = _context.Cities.FirstOrDefault(x => x.City1 == cityName);


                CityDto cityDto = _mapper.Map<CityDto>(c);

                return CreatedAtAction(nameof(GetCities), new { id = city.Idcity }, cityDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the city.");
            }

        }
    }
}
