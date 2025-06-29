using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IAuthRepository
    {
        public User GetUserByEmail(string email);
        public List<string> GetUserRole(int id);
        public void AddUser(User user);
        public bool UserExists(string email);
        public void SaveChanges();
    }
}
