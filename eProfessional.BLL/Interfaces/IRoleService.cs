using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IRoleService
    {
        public string GetCurrentUserRole();
        public List<RoleDto> GetUserRole();
        public bool AssignRoleToUser(RoleDto roleDto);
        public bool DeleteRole(int roleId);
        public bool UpdateRole(RoleDto newRole);

    }
}
