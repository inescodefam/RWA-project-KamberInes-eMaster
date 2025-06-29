
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {

        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("get/{n}")]
        public ActionResult<IEnumerable<LogApiDto>> GetLogs(int n)
        {
            try
            {
                var logs = _logService.GetLogs(n);
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
