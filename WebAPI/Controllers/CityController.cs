using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Context;

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
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var result = await _cityService.GetAllCitiesAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> GetCities(int count, int start = 0, string searchTerm = "")
        {
            try
            {
                var result = await _cityService.GetCitiesAsync(searchTerm, count, start);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CityDto>> CreateCity([FromBody] CreateCityDto city)
        {
            if (city.Name == null)
            {
                return BadRequest("City data is required.");
            }

            var cities = await _cityService.GetAllCitiesAsync();
            if (cities.Any(c => c.Name.Equals(city.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"City '{city.Name}' already exists.");
                //return Conflict($"City '{city.Name}' already exists.");
            }

            try
            {
                var cityDto = await _cityService.CreateCityAsync(city.Name);

                return CreatedAtAction(nameof(GetCities), new { id = cityDto.Idcity }, cityDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the city.");
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCity(int id, string name)
        {
            var response = await _cityService.UpdateCityAsync(id, name);
            return Ok("City updated successfuly!");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity(int id)
        {
            try
            {
                var response = await _cityService.DeleteCityAsync(id);
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
