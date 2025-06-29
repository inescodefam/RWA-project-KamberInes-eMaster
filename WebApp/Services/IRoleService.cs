using Shared.BL.DTOs;
using Shared.BL.Models;

namespace Shared.BL.Services
{
    public interface IRoleService
    {
        public string GetCurrentUserRole();
        public List<Role> GetUserRole();
        public bool AssignRoleToUser(RoleDto roleDto);
        public bool DeleteRole(int roleId);
        public bool UpdateRole(int roleId, string newRoleName);

    }
}
