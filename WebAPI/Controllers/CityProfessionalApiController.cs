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
    public class CityProfessionalApiController : ControllerBase
    {

        private readonly ICityProfessionalService _cityProfessionalService;
        private readonly IMapper _mapper;

        public CityProfessionalApiController(ICityProfessionalService cityProfessionalService,
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
                if (result == null || !result.Any())
                {
                    return NotFound("No city-professionals found.");
                }
                var cityProfessionalDataDtos = _mapper.Map<List<CityProfessionalDataApiDto>>(result);
                return Ok(cityProfessionalDataDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("city")]
        public IActionResult GetProfessionalsFromCity([FromQuery] int cityId)
        {
            try
            {
                var result = _cityProfessionalService.GetProfessionalsByCity(cityId);
                if (result == null || !result.Any())
                {
                    return NotFound("No professionals found for the given city.");
                }
                var professionalDataDtos = _mapper.Map<List<ProfessionalApiDataDto>>(result);
                return Ok(professionalDataDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("professional")]
        public IActionResult GetCitiesByProfessional([FromQuery] int professionalId)
        {
            try
            {
                var result = _cityProfessionalService.GetCitiesByProfessional(professionalId);
                if (result == null || !result.Any())
                {
                    return NotFound("No cities found for the given professional.");
                }
                var cityDtos = _mapper.Map<List<CityApiDto>>(result);
                return Ok(cityDtos);
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
                var cityProfessionalApiDto = _mapper.Map<CityProfessionalApiDto>(result);
                return Ok(cityProfessionalApiDto);
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

                var cityProfessionalDataApiDto = _mapper.Map<CityProfessionalDataApiDto>(result);
                return Ok(cityProfessionalDataApiDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("professional/{professionalId}")]
        public IActionResult UpdateCitiesByProfessionalId(int professionalId, [FromBody] List<int> citiesIds)
        {
            try
            {
                var result = _cityProfessionalService.UpdateCitiesByProfessional(professionalId, citiesIds);

                if (result == null || !result.Any())
                {
                    return NotFound("No cities found for the given professional.");
                }
                var cityProfessionalDataDtos = _mapper.Map<List<CityProfessionalDataApiDto>>(result);
                return Ok(cityProfessionalDataDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("city/{cityId}")]
        public IActionResult UpdateProfessionalsByCityId(int cityId, [FromBody] List<int> professionalsIds)
        {
            try
            {

                var result = _cityProfessionalService.UpdateProfessionalsByCity(cityId, professionalsIds);

                if (result == null || !result.Any())
                {
                    return NotFound("No professionals found for the given city.");
                }
                var cityProfessionalDataDtos = _mapper.Map<List<CityProfessionalDataApiDto>>(result);
                return Ok(cityProfessionalDataDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("professional/{id}")]
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
        [HttpDelete("city/{Id}")]
        public IActionResult DeletePRofessionalsForCities(int Id)
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
