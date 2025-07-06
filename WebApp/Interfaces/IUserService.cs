using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IUserService
    {
        List<UserVM> GetUsers(int count, int start);
        public List<UserVM> GetAllUsers();
        UserVM GetUserById(int id);
        UserVM GetUserByEmail(string email);
        UserVM GetUserByUsername(string username);
        bool UpdateUser(UserVM userDto);
        bool DeleteUser(int userId);
    }
}
