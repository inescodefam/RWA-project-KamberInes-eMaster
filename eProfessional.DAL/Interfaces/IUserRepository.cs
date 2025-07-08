using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IUserRepository : ICrudRepository<User>
    {
        public List<User> GetUsers(int count, int start = 0);
        public List<User> GetUsers();
        public User GetUserByEmail(string email);
        public User GetUserByUsername(string username);
        public List<User> Search(string role, string username, int pageSize, int page);
        public int SearchTotal(string role, string username);
    }
}
