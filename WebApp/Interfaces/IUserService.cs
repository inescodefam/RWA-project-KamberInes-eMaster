using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IUserService
    {
        List<UserVM> GetUsers(int count, int start = 0);
        UserVM GetUserById(int id);
        UserVM GetUserByEmail(string email);
        bool UpdateUser(UserVM userDto);
        bool DeleteUser(int userId);
    }
}
