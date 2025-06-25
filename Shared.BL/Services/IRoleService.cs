namespace Shared.BL.Services
{
    public interface IRoleService
    {
        public Task<string> GetUserRole();
        public Task<bool> AssignRoleToUser(string roleName);
    }
}
