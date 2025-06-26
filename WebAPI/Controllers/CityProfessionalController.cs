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
        public async Task<IActionResult> Create([FromBody] CityProfessionalDto model)
        {

            try
            {
                var result = await _cityProfessionalService.AddCityProfessionalAsync(model);
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
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _cityProfessionalService.GetCityProfessionalsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetCitiesForProfessional(int professionalId)
        {
            try
            {
                var result = await _cityProfessionalService.GetCitysByProfessionalAsync(professionalId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("city/{cityId}")]
        public async Task<IActionResult> GetProfessionalsFromCity(int cityId)
        {
            try
            {
                var result = await _cityProfessionalService.GetProfessionalsByCityAsync(cityId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CityProfessionalDto model)
        {
            try
            {
                var result = await _cityProfessionalService.UpdateCityProfessionalAsync(id, model);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _cityProfessionalService.DeleteCityProfessionalAsync(id);
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
