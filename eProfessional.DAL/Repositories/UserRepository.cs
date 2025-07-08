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

        public User GetUserByUsername(string username)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower().Trim());

            if (user == null)
            {
                throw new Exception($"User with email {username} not found.");
            }
            return user;
        }

        public List<User> GetUsers(int count, int start = 0)
        {
            return _context.Users.Skip(start * count).Take(count).ToList();
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public List<User> Search(string role, string username, int pageSize, int page)
        {
            if (pageSize < 1) pageSize = 10;
            var roles = new List<int>();
            if (!string.IsNullOrEmpty(role))
            {
                roles = _context.Roles
                    .Where(r => r.RoleName.ToLower() == role.ToLower())
                    .Select(r => r.UserId)
                    .ToList();
            }

            var users = new List<User>();
            if (!string.IsNullOrEmpty(username))
            {
                users = _context.Users
                    .Where(u => u.Username.ToLower().Contains(username.ToLower()))
                    .ToList();

                if (roles.Count > 0)
                {
                    users = users.Where(u => roles.Contains(u.Iduser)).ToList();
                }
            }
            else
            {
                users = _context.Users.ToList();
                if (roles.Count > 0)
                {
                    users = users.Where(u => roles.Contains(u.Iduser)).ToList();
                }
            }

            return users.Skip(page * pageSize).Take(pageSize).ToList();
        }

        public int SearchTotal(string role, string username)
        {
            var result = 0;
            var roles = new List<int>();
            var users = new List<User>();
            if (!string.IsNullOrEmpty(role))
            {
                roles = _context.Roles.Where(r => r.RoleName.ToLower() == role.ToLower()).Select(r => r.UserId).ToList();
                result = roles.Count;
            }

            if (!string.IsNullOrEmpty(username))
            {
                users = _context.Users.Where(u => u.Username.ToLower().Contains(username.ToLower())).ToList();

                if (roles.Count > 0)
                {
                    result = users.Count(u => roles.Contains(u.Iduser));
                }
                else
                {
                    result = users.Count;
                }
            }

            return result;
        }

    }
}
