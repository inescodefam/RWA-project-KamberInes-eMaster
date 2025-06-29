using eProfessional.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly EProfessionalContext _context;

        public LogController(EProfessionalContext context)
        {
            _context = context;
        }

        [HttpGet("get/{n}")]
        public ActionResult<IEnumerable<LogApiDto>> GetLogs(int n)
        {
            try
            {
                var logs = _context.Logs
                    .OrderByDescending(log => log.LogTimeStamp)
                    .Take(n)
                    .ToList();
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
            var count = _context.Logs.Count();
            return Ok(count);
        }
    }
}
