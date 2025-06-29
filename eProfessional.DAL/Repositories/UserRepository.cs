using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class UserRepository : CrudRepository<User>, IUserRepository
    {
        public UserRepository(EProfessionalContext context) : base(context)
        { }

        public User GetUserByEmail(string email)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower().Trim());

            if (user == null)
            {
                throw new Exception($"User with email {email} not found.");
            }
            return user;
        }

        public List<User> GetUsers(int count, int start = 0)
        {
            return _context.Users.Skip(start * count).Take(count).ToList();
        }
    }

}
