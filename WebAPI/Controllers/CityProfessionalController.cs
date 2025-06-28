using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;

namespace WebAPI.Controllers
{
    [Route("api/city-professional")]
    [ApiController]
    public class CityProfessionalController : ControllerBase
    {

        private readonly ICityProfessionalService _cityProfessionalService;

        public CityProfessionalController(ICityProfessionalService cityProfessionalService)
        {
            _cityProfessionalService = cityProfessionalService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CityProfessionalDto model)
        {

            try
            {
                var result = _cityProfessionalService.AddCityProfessional(model);
                if (result == null)
                {
                    return BadRequest("CityProfessional could not be created.");
                }
                return CreatedAtAction(nameof(Get), new { id = result.IdProfessionalCity }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        public IActionResult Get()
        {
            try
            {
                var result = _cityProfessionalService.GetCityProfessionals();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("professional/{professionalId}")]
        public IActionResult GetCitiesForProfessional(int professionalId)
        {
            try
            {
                var result = _cityProfessionalService.GetCitysByProfessional(professionalId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("city/{cityId}")]
        public IActionResult GetProfessionalsFromCity(int cityId)
        {
            try
            {
                var result = _cityProfessionalService.GetProfessionalsByCity(cityId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CityProfessionalDto model)
        {
            try
            {
                var result = _cityProfessionalService.UpdateCityProfessional(id, model);
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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
