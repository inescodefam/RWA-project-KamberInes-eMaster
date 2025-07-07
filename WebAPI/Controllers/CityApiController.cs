using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/city")]
    [ApiController]
    [Authorize]
    public class CityApiController : Controller
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;

        public CityApiController(IMapper mapper, ICityService cityService)
        {
            _cityService = cityService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public IActionResult GetAllCities()
        {
            try
            {
                var result = _cityService.GetAllCities();

                if (result == null || !result.Any())
                {
                    return NotFound("No cities found.");
                }
                var cityDtos = _mapper.Map<List<CityApiDto>>(result);
                return Ok(cityDtos);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the cities.");
            }
        }

        [HttpGet("count")]
        public ActionResult<int> GetCityCount(string? searchTerm = null)
        {
            try
            {
                var count = _cityService.GetCityCount(searchTerm);
                return Ok(count);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the city count.");
            }
        }

        [HttpGet]
        public ActionResult<List<CityApiDto>> GetCities(int count, int start = 0, string searchTerm = "")
        {
            try
            {
                var result = _cityService.SearchCities(searchTerm, count, start);
                if (result == null || !result.Any())
                {
                    return NotFound("No cities found.");
                }
                var cityDtos = _mapper.Map<List<CityApiDto>>(result);

                return Ok(cityDtos);
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

            try
            {
                var cityDto = _cityService.CreateCity(city.Name);

                return CreatedAtAction(nameof(GetCities), new { id = cityDto.Idcity }, cityDto.Name);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }

        [HttpPut]
        public ActionResult UpdateCity([FromBody] CityApiDto city)
        {
            if (string.IsNullOrEmpty(city.Name) || city.Idcity == null || city.Idcity <= 0)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var cityDto = _mapper.Map<CityDto>(city);
                var response = _cityService.UpdateCity(cityDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

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
