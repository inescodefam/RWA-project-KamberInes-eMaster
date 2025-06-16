using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.Models;
using System.Security.Claims;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RoleController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly EProfessionalContext _context;
        public RoleController(IConfiguration configuration, EProfessionalContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Authorize]
        [HttpGet("api/role")]
        public IActionResult GetUserRole()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRole))
            {
                return Unauthorized("User role not found.");
            }
            return Ok(new { Role = userRole });
        }

        [HttpPost("api/role")]
        public IActionResult AssignRoleToUser([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            _context.Roles.Add(new Role
            {
                RoleName = roleName,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0")
            });

            return Ok();
        }
    }
}
