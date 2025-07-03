﻿using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/professional")]
    [Authorize]
    [ApiController]
    public class ProfessionalApiController : ControllerBase
    {
        private readonly ILogService _loggingService;
        private readonly IMapper _mapper;
        private readonly IProfessionalService _professionalService;
        public ProfessionalApiController(
            ILogService logService,
            IProfessionalService professionalService,
            IMapper mapper)
        {
            _loggingService = logService;
            _professionalService = professionalService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProfessionalApiDataDto>> GetAllProfessionals(int count, int start = 0)
        {
            if (count <= 0 || start < 0)
                return BadRequest("Invalid paging parameters");
            try
            {
                var professionals = _professionalService.GetProfessionals(count, start);

                if (professionals == null || !professionals.Any())
                {
                    _loggingService.CreateLog("No professionals found in the database.", "info");
                    return NoContent();
                }
                _mapper.Map<IEnumerable<ProfessionalApiDataDto>>(professionals);

                _loggingService.CreateLog($"Retrieved {professionals.Count} professionals from the database.", "info");

                var professionalDataApiDtos = _mapper.Map<List<ProfessionalApiDataDto>>(professionals);
                return Ok(professionalDataApiDtos);
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error retrieving professionals: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProfessionalsController>/5
        [HttpGet("{id}")]
        public ActionResult<ProfessionalApiDataDto> GetSingleProfessionalById(int id)
        {
            try
            {
                var professionalDto = _professionalService.GetSingleProfessional(id);
                _loggingService.CreateLog($"Retrieved professional with ID {id}.", "info");

                if (professionalDto == null)
                {
                    _loggingService.CreateLog($"Professional with ID {id} not found.", "info");
                    return NotFound();
                }
                var professionalApiDto = _mapper.Map<ProfessionalApiDataDto>(professionalDto);
                return Ok(professionalApiDto);
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error retrieving professional with ID {id}: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // get professional by name name in user
        [HttpGet("search")]
        public ActionResult<IEnumerable<ProfessionalApiDataDto>> Search(
            [FromQuery] string? name,
            [FromQuery] string? serviceType,
            [FromQuery] int count,
            [FromQuery] int start = 0
            )
        {
            try
            {
                var professionalDtos = _professionalService.Search(name, serviceType, count, start);

                if (professionalDtos == null || !professionalDtos.Any())
                {
                    _loggingService.CreateLog("No professionals found matching the search criteria.", "info");
                    return NoContent();
                }
                _loggingService.CreateLog($"Found {professionalDtos.Count} professionals matching the search criteria.", "info");
                var professionalDtosMapped = _mapper.Map<List<ProfessionalApiDataDto>>(professionalDtos);
                return Ok(professionalDtosMapped);

            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error retrieving professionals by name '': {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ProfessionalsController>
        [HttpPost]
        public IActionResult Post([FromBody] ProfessionalApiDto professionalApiDataDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var professionalDataDto = _mapper.Map<ProfessionalBaseDto>(professionalApiDataDto);

                var response = _professionalService.CreateProfessional(professionalDataDto);
                if (response == null)
                {
                    _loggingService.CreateLog("Failed to create professional.", "error");
                    return BadRequest("Failed to create professional.");
                }
                _loggingService.CreateLog("Professional created successfully.", "info");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error adding professional: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProfessionalsController>/5
        [HttpPut]
        public IActionResult Put([FromBody] ProfessionalApiDataDto professionalApiDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var professionalDto = _mapper.Map<ProfessionalDataDto>(professionalApiDto);
                var response = _professionalService.UpdateProfessional(professionalDto);
                if (!response)
                {
                    _loggingService.CreateLog($"Failed to update professional with ID {professionalApiDto.IdProfessional}.", "error");
                    return NotFound();
                }

                _loggingService.CreateLog($"Professional with ID {professionalApiDto.IdProfessional} updated successfully.", "info");
                return Ok("Professional updated successfully");
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error updating professional with ID {professionalApiDto.IdProfessional}: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ProfessionalsController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = _professionalService.DeleteProfessional(id);
                if (!response)
                {
                    _loggingService.CreateLog($"Failed to delete professional with ID {id}.", "error");
                    return NotFound();
                }
                _loggingService.CreateLog($"Professional with ID {id} deleted successfully.", "info");
                return Ok("Professional deleted successfully");
            }
            catch (Exception ex)
            {
                _loggingService.CreateLog($"Error deleting professional with ID {id}: {ex.Message}", "error");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
