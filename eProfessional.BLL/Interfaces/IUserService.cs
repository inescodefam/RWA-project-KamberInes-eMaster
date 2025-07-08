using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IUserService
    {
        List<UserDto> GetUsers(int count, int start = 0);
        List<UserDto> GetUsers();
        List<UserDto> Search(string role, string username, int pageSize, int page);
        int SearchTotal(string role, string username);
        UserDto GetUserById(int id);
        UserDto GetUserByEmail(string email);
        UserDto GetUserByUsername(string username);
        UserDto UpdateUser(UserDto userDto);
        void DeleteUser(int userId);
    }
}
