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
        public ActionResult<List<CityDto>> GetCities(int count, int start = 0, string searchTerm = "")
        {
            try
            {
                var query = _context.Cities.AsQueryable();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(c => c.Name.Contains(searchTerm));
                }
                var result = query.Skip(start).Take(count).ToList();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }

        [HttpPost]
        public ActionResult<CityDto> CreateCity([FromBody] string cityName)
        {
            System.Diagnostics.Debug.WriteLine($"Received cityName: {cityName}");
            if (cityName == null)
            {
                return BadRequest("City data is required.");
            }
            try
            {
                if (_context.Cities.Any(x => x.Name == cityName))
                {
                    return Conflict("City already exists.");
                }

                var city = new City
                {
                    Name = cityName,

                };
                _context.Cities.Add(city);
                _context.SaveChanges();

                City c = _context.Cities.FirstOrDefault(x => x.Name == cityName);


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
