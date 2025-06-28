using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Context;

namespace WebAPI.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize]
    public class RoleController : Controller
    {

        private readonly IRoleService _roleService;

        public RoleController(IConfiguration configuration, EProfessionalContext context, IRoleService roleService)
        {

            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult GetUserRole()
        {
            var userRole = _roleService.GetUserRole();
            if (userRole == null)
            {
                return Unauthorized("User role not found.");
            }

            return Ok(new { Role = userRole });
        }

        [HttpPost]
        public IActionResult AssignRoleToUser(RoleDto roleDto)
        {
            if (string.IsNullOrEmpty(roleDto.RoleName) || roleDto.UserId == null)
            {
                return BadRequest("Role name cannot be empty.");
            }

            var response = _roleService.AssignRoleToUser(roleDto);

            if (!response)
            {
                return BadRequest("Failed to assign role to user.");
            }

            return Ok();
        }

        [HttpPut("api/role/{roleId}")]
        public IActionResult UpdateRole(int roleId, [FromBody] string newRoleName)
        {
            if (string.IsNullOrEmpty(newRoleName))
            {
                return BadRequest("New role name cannot be empty.");
            }
            var response = _roleService.UpdateRole(roleId, newRoleName);
            if (!response)
            {
                return NotFound("Role not found or could not be updated.");
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("api/role/{roleId}")]
        public IActionResult DeleteRole(int roleId)
        {
            var response = _roleService.DeleteRole(roleId);
            if (!response)
            {
                return NotFound("Role not found or could not be deleted.");
            }
            return Ok();
        }
    }
}
