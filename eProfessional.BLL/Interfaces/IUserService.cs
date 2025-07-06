using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IUserService
    {
        List<UserDto> GetUsers(int count, int start = 0);
        List<UserDto> GetUsers();
        UserDto GetUserById(int id);
        UserDto GetUserByEmail(string email);
        UserDto GetUserByUsername(string username);
        UserDto UpdateUser(UserDto userDto);
        void DeleteUser(int userId);
    }
}
