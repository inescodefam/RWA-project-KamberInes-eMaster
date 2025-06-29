using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class RoleRepository : CrudRepository<Role>, IRoleRepository
    {
        public RoleRepository(EProfessionalContext context) : base(context)
        { }

        public bool AddUserRole(Role role)
        {
            var user = _context.Users.FirstOrDefault(u => u.Iduser == role.UserId);

            if (role == null || string.IsNullOrEmpty(role.RoleName) || user == null)
            {
                return false;
            }

            _dbSet.Add(role);
            _context.SaveChanges();
            return true;
        }
    }
}
