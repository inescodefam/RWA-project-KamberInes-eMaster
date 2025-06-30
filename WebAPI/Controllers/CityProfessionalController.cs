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
        public IActionResult Update([FromBody] CreateCityProfessionalApiDto model)
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

        [HttpPut("professional/{professionalId}")]
        public IActionResult UpdateCitiesByProfessionalId(int professionalId, [FromBody] List<CreateCityApiDto> citiesDtos)
        {
            try
            {
                var cityDtos = _mapper.Map<List<CityDto>>(citiesDtos);
                var result = _cityProfessionalService.UpdateCitiesByProfessionalId(professionalId, cityDtos);
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

        [HttpPut("city/{cityId}")]
        public IActionResult UpdateProfessionalsByCityId(int cityId, [FromBody] List<ProfessionalApiDto> professionalsDtos)
        {
            try
            {
                var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionalsDtos);
                var result = _cityProfessionalService.UpdateProfessionalsByCityId(cityId, professionalDtos);
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
        [HttpDelete("professional/{professionalId}")]
        public IActionResult DeleteCitiesForProfessional(int id)
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
        [HttpDelete("city/{cityId}")]
        public IActionResult DeletePRofessionalsForCities(int cityId)
        {
            try
            {
                var result = _cityProfessionalService.DeleteProfessionalsForCity(cityId);
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
