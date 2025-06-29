using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IRoleRepository : ICrudRepository<Role>
    {
        public bool AddUserRole(Role role);
    }
}
