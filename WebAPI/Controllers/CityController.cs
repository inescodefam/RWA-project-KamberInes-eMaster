using AutoMapper;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/city")]
    [ApiController]
    [Authorize]
    public class CityController : Controller
    {
        private readonly ICityService _cityService;

        public CityController(EProfessionalContext context, IMapper mapper, ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("all")]
        public IActionResult GetAllCities()
        {
            try
            {
                var result = _cityService.GetAllCities();
                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }

        [HttpGet]
        public ActionResult<List<CityApiDto>> GetCities(int count, int start = 0, string searchTerm = "")
        {
            try
            {
                var result = _cityService.GetCities(searchTerm, count, start);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }

        [HttpPost]
        public ActionResult<CityApiDto> CreateCity([FromBody] CreateCityApiDto city)
        {
            if (city.Name == null)
            {
                return BadRequest("City data is required.");
            }

            var cities = _cityService.GetAllCities();
            if (cities.Any(c => c.Name.Equals(city.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"City '{city.Name}' already exists.");
            }

            try
            {
                var cityDto = _cityService.CreateCity(city.Name);

                return CreatedAtAction(nameof(GetCities), new { id = cityDto.Idcity }, cityDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the city.");
            }

        }

        [HttpPut("{id}")]
        public ActionResult UpdateCity(int id, string name)
        {
            var response = _cityService.UpdateCity(id, name);
            return Ok("City updated successfuly!");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteCity(int id)
        {
            try
            {
                var response = _cityService.DeleteCity(id);
                if (!response)
                {
                    return NotFound();
                }
                return Ok("City deleted succesfuly.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
