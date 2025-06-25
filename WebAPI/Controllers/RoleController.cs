using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.Services;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RoleController : Controller
    {

        private readonly IRoleService _roleService;

        public RoleController(IConfiguration configuration, EProfessionalContext context, IRoleService roleService)
        {

            _roleService = roleService;
        }

        [Authorize]
        [HttpGet("api/role")]
        public async Task<IActionResult> GetUserRole()
        {
            var userRole = await _roleService.GetUserRole();
            if (userRole == null)
            {
                return Unauthorized("User role not found.");
            }

            return Ok(new { Role = userRole });
        }

        [HttpPost("api/role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var response = await _roleService.AssignRoleToUser(roleName);

            if (!response)
            {
                return BadRequest("Failed to assign role to user.");
            }

            return Ok();
        }
    }
}
