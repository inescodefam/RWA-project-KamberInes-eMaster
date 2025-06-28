using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface IUserService
    {
        List<UserDto> GetUsers(int count, int start = 0);
        UserDto GetUserById(int id);
        UserDto GetUserByEmail(string email);
        bool UpdateUser(UserDto userDto);
        void DeleteUser(int userId);
    }
}
