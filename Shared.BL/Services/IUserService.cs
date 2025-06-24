using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsers(int count, int start = 0);
        Task<UserDto> GetUserById(int id);
        Task<UserDto> GetUserByEmail(string email);
        Task<bool> UpdateUser(UserDto userDto);
        void DeleteUser(UserDto userDto);
    }
}
