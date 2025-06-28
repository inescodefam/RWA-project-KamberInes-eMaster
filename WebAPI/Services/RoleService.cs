using Microsoft.IdentityModel.Tokens;
using Shared.BL.DTOs;
using Shared.BL.Models;
using Shared.BL.Services;
using System.Security.Claims;
using WebAPI.Context;

namespace WebAPI.Services
{
    public class RoleService : IRoleService
    {

        private readonly IConfiguration _configuration;
        private readonly EProfessionalContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(IConfiguration configuration, EProfessionalContext context, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserRole()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userRole = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRole))
            {
                return "";
            }

            return userRole;
        }

        public List<Role> GetUserRole()
        {
            var userRole = _context.Roles.ToList();
            if (userRole == null || userRole.Count() == 0)
            {
                return new List<Role>();
            }

            return userRole;
        }

        public bool AssignRoleToUser(RoleDto roleDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Iduser == roleDto.UserId);

            if (user == null || roleDto.RoleName.IsNullOrEmpty())
            {
                return false;
            }

            _context.Roles.Add(new Role
            {
                RoleName = roleDto.RoleName,
                UserId = roleDto.UserId,
            });
            _context.SaveChanges();

            return true;
        }

        public bool UpdateRole(int roleId, string newRoleName)
        {
            Role roleToUpdate = _context.Roles.FirstOrDefault(r => r.Idrole == roleId);
            if (roleToUpdate == null)
            {
                return false;
            }
            roleToUpdate.RoleName = newRoleName;
            _context.Roles.Update(roleToUpdate);
            _context.SaveChanges();

            return true;
        }

        public bool DeleteRole(int roleId)
        {
            Role roletoDelete = _context.Roles.FirstOrDefault(r => r.Idrole == roleId);
            if (roletoDelete == null)
            {
                return false;
            }

            _context.Roles.Remove(roletoDelete);
            _context.SaveChanges();
            return true;
        }

    }
}
