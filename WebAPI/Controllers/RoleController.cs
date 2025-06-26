using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.Services;
using WebAPI.Context;

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

        [HttpPut("api/role/{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] string newRoleName)
        {
            if (string.IsNullOrEmpty(newRoleName))
            {
                return BadRequest("New role name cannot be empty.");
            }
            var response = await _roleService.UpdateRole(roleId, newRoleName);
            if (!response)
            {
                return NotFound("Role not found or could not be updated.");
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("api/role/{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var response = await _roleService.DeleteRole(roleId);
            if (!response)
            {
                return NotFound("Role not found or could not be deleted.");
            }
            return Ok();
        }
    }
}
