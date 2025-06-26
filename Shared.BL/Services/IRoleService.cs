using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface IRoleService
    {
        public string GetUserRole();
        public Task<bool> AssignRoleToUser(RoleDto roleDto);
        public Task<bool> DeleteRole(int roleId);
        public Task<bool> UpdateRole(int roleId, string newRoleName);

    }
}
