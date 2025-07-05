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
    public class RoleApiController : Controller
    {

        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleApiController(IRoleService roleService, IMapper mapper)
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
            var userRoleDtos = _mapper.Map<List<RoleApiDto>>(userRole);


            return Ok(userRoleDtos);
        }

        [HttpPost]
        public IActionResult AssignRoleToUser(RoleApiDto roleApiDto)
        {
            if (string.IsNullOrEmpty(roleApiDto.RoleName) || roleApiDto?.UserId == null)
            {
                return BadRequest("Role name cannot be empty.");
            }

            try
            {
                var roleDto = _mapper.Map<RoleDto>(roleApiDto);
                var response = _roleService.AssignRoleToUser(roleDto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to assign role to user. Error: {ex.Message}");
            }

        }

        [HttpPut("api/role/{roleId}")]
        public IActionResult UpdateRole([FromBody] RoleApiDto newRole)
        {
            if (string.IsNullOrEmpty(newRole.RoleName) ||
                newRole.UserId <= 0 || newRole.Idrole <= 0)

            {
                return BadRequest("New role name cannot be empty.");
            }
            RoleDto newRoleDto = _mapper.Map<RoleDto>(newRole);

            var response = _roleService.UpdateRole(newRoleDto);
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
