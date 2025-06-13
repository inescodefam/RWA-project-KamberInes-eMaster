using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.Models;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly EProfessionalContext _context;

        public ServiceController(EProfessionalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Service>> GetServices(int count, int start = 0)
        {
            try
            {
                var services = _context.Services
                    .Skip(start * count)
                    .Take(count)
                    .ToList();
                return Ok(services);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the services.");
            }
        }
    }
}
