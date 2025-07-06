
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {

        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("get/{pageSize}")]
        public ActionResult<IEnumerable<LogApiDto>> GetLogs(int pageSize, int page)
        {
            try
            {
                var logs = _logService.GetLogs(pageSize, page);
                if (logs == null || !logs.Any())
                {
                    return NotFound("No logs found.");
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving logs: {ex.Message}");
            }

        }

        [HttpGet("count")]
        public IActionResult GetLogCount()
        {
            var count = _logService.GetLogCount();
            return Ok(count);
        }
    }
}
