using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/servicetype")]
    [ApiController]
    [Authorize]
    public class ServiceTypeController : Controller
    {
        private readonly EProfessionalContext _context;

        public ServiceTypeController(EProfessionalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ServiceTypeDto>> GetServiceTypes(int count, int start = 0)
        {
            try
            {
                var serviceTypes = _context.ServiceTypes
                    .Skip(start * count)
                    .Take(count)
                    .Select(st => new ServiceTypeDto
                    {
                        IdserviceType = st.IdserviceType,
                        ServiceTypeName = st.ServiceTypeName
                    })
                    .ToList();
                return Ok(serviceTypes);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the service types.");
            }
        }
    }
}
