using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IUserRepository : ICrudRepository<User>
    {
        public List<User> GetUsers(int count, int start = 0);
        public User GetUserByEmail(string email);
    }
}
