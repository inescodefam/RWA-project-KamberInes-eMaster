using Microsoft.IdentityModel.Tokens;
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

        public async Task<string> GetUserRole()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRole))
            {
                return null;
            }

            return userRole;
        }

        public async Task<bool> AssignRoleToUser(string roleName)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated || roleName.IsNullOrEmpty())
            {
                return false;
            }

            _context.Roles.Add(new Role
            {
                RoleName = roleName,
                UserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0")
            });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRole(int roleId, string newRoleName)
        {
            Role roleToUpdate = _context.Roles.FirstOrDefault(r => r.Idrole == roleId);
            if (roleToUpdate == null)
            {
                return false;
            }
            roleToUpdate.RoleName = newRoleName;
            _context.Roles.Update(roleToUpdate);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            Role roletoDelete = _context.Roles.FirstOrDefault(r => r.Idrole == roleId);
            if (roletoDelete == null)
            {
                return false;
            }

            _context.Roles.Remove(roletoDelete);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
