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

        [HttpPost]
        public IActionResult Create([FromBody] CityProfessionalApiDto model)
        {

            try
            {
                var cityProfessionalDto = _mapper.Map<CityProfessionalDto>(model);
                var result = _cityProfessionalService.AddCityProfessional(cityProfessionalDto);

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

        [HttpGet("professional/{professionalId}")]
        public IActionResult GetCitiesByProfessionalId(int professionalId)
        {
            try
            {
                var result = _cityProfessionalService.GetCitiesByProfessionalId(professionalId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CityProfessionalApiDto model)
        {
            try
            {
                var cityProfessionalDto = _mapper.Map<CityProfessionalDto>(model);
                var result = _cityProfessionalService.UpdateCityProfessional(id, cityProfessionalDto);
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
