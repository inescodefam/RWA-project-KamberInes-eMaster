namespace Shared.BL.Services
{
    public interface IRoleService
    {
        public string GetUserRole();
        public Task<bool> AssignRoleToUser(string roleName);
        public Task<bool> DeleteRole(int roleId);
        public Task<bool> UpdateRole(int roleId, string newRoleName);

    }
}
