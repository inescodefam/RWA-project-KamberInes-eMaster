using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly EMasterContext _context;

        public LogController(EMasterContext context)
        {
            _context = context;
        }

        [HttpGet("get/{n}")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogs(int n)
        {
            try
            {
                var logs = await _context.Logs
                    .OrderByDescending(log => log.LogTimeStamp)
                    .Take(n)
                    .ToListAsync();
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
        public async Task<IActionResult> GetLogCount()
        {
            var count = await _context.Logs.CountAsync();
            return Ok(count);
        }
    }
}
