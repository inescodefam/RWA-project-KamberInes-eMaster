using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        protected readonly EProfessionalContext _context;

        public AuthRepository(EProfessionalContext context)
        {
            _context = context;
        }

        public User GetUserByEmail(string email)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public List<string> GetUserRole(int id)
        {
            var roles = _context.Roles
                    .Where(r => r.UserId == id)
                    .Select(r => r.RoleName)
                    .ToList();
            return roles ?? new List<string>();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
