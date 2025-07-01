using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/city-professional")]
    [ApiController]
    [Authorize]
    public class CityProfessionalController : ControllerBase
    {

        private readonly ICityProfessionalService _cityProfessionalService;
        private readonly IMapper _mapper;

        public CityProfessionalController(ICityProfessionalService cityProfessionalService,
            IMapper mapper)
        {
            _cityProfessionalService = cityProfessionalService;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult Get(int count, int start = 0)
        {
            try
            {
                var result = _cityProfessionalService.GetCityProfessionals(count, start);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("city")]
        public IActionResult GetProfessionalsFromCity([FromQuery] string cityName)
        {
            try
            {
                var result = _cityProfessionalService.GetProfessionalsByCity(cityName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("professional")]
        public IActionResult GetCitiesByProfessional([FromQuery] string professionalName)
        {
            try
            {
                var result = _cityProfessionalService.GetCitiesByProfessional(professionalName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCityProfessionalApiDto model)
        {

            try
            {
                var cityProfessionalDto = _mapper.Map<CityProfessionalDto>(model);
                var result = _cityProfessionalService.AddCityProfessional(cityProfessionalDto);

                if (result == null)
                {
                    return BadRequest("CityProfessional could not be created.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] CityProfessionalApiDto model)
        {
            try
            {
                var cityProfessionalDto = _mapper.Map<CityProfessionalDto>(model);
                var result = _cityProfessionalService.UpdateCityProfessional(cityProfessionalDto);
                if (result == null)
                {
                    return NotFound("CityProfessional not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("professional")]
        public IActionResult UpdateCitiesByProfessionalId(int professionalId, [FromBody] List<int> citiesIds)
        {
            try
            {
                var result = _cityProfessionalService.UpdateCitiesByProfessional(professionalId, citiesIds);

                if (result == null || !result.Any())
                {
                    return NotFound("No cities found for the given professional.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("city")]
        public IActionResult UpdateProfessionalsByCityId(int cityId, [FromBody] List<int> professionalsIds)
        {
            try
            {

                var result = _cityProfessionalService.UpdateProfessionalsByCity(cityId, professionalsIds);

                if (result == null || !result.Any())
                {
                    return NotFound("No professionals found for the given city.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("professional")]
        public IActionResult DeleteCitiesForProfessional([FromQuery] int id)
        {
            try
            {
                var result = _cityProfessionalService.DeleteCitiesForProfessional(id);
                if (!result)
                {
                    return NotFound("Reference not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("city")]
        public IActionResult DeletePRofessionalsForCities([FromQuery] int Id)
        {
            try
            {
                var result = _cityProfessionalService.DeleteProfessionalsForCity(Id);
                if (!result)
                {
                    return NotFound("Reference not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            try
            {
                var result = _cityProfessionalService.DeleteCityProfessional(id);
                if (!result)
                {
                    return NotFound("Reference not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
