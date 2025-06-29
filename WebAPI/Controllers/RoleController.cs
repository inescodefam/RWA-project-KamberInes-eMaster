using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize]
    public class RoleController : Controller
    {

        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {

            _roleService = roleService;
            _mapper = mapper;
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
        public IActionResult AssignRoleToUser(RoleApiDto roleApiDto)
        {
            if (string.IsNullOrEmpty(roleApiDto.RoleName) || roleApiDto.UserId == null)
            {
                return BadRequest("Role name cannot be empty.");
            }

            var roleDto = _mapper.Map<RoleDto>(roleApiDto);
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
            RoleDto newRole = new RoleDto
            {
                Idrole = roleId,
                RoleName = newRoleName
            };

            var response = _roleService.UpdateRole(newRole);
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
