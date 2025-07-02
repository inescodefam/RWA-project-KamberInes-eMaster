using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IRoleService
    {
        public List<RoleVM> GetUserRole();
        public bool AssignRoleToUser(RoleVM roleDto);
        public bool UpdateRole(RoleVM roleVM);
        public bool DeleteRole(int roleId);

    }
}
