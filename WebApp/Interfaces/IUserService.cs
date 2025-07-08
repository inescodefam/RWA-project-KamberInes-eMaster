using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IUserService
    {
        List<UserVM> GetUsers(int count, int start);
        public List<UserVM> GetAllUsers();
        public List<UserVM> Search(string role, string username, int pageSize, int page);
        public int SearchTotal(string role, string username);
        UserVM GetUserById(int id);
        UserVM GetUserByEmail(string email);
        UserVM GetUserByUsername(string username);
        bool UpdateUser(UserVM userDto);
        bool DeleteUser(int userId);
    }
}
