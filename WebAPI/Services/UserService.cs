using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Auth;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;

        public UserService(EProfessionalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetUsers(int count, int start = 0)
        {
            var users = _context.Users.Skip(start * count).Take(count).ToList();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Iduser == id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public bool UpdateUser(UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == userDto.Email);
            if (user == null) return false;

            var b64salt = HashPwd.GetSalt();
            var b64hash = HashPwd.GetHash(userDto.Password, b64salt);

            user.Username = userDto.Username ?? user.Username;
            user.FirstName = userDto.FirstName ?? user.FirstName;
            user.LastName = userDto.LastName ?? user.LastName;
            user.Email = userDto.Email;
            user.Phone = userDto.PhoneNumber ?? user.Phone;
            user.PasswordHash = b64hash;
            user.PasswordSalt = b64salt;
            _context.SaveChanges();
            return true;
        }

        public void DeleteUser(UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Iduser == userDto.Iduser);
            if (user == null) throw new Exception("User not found");

            _context.Users.Remove(user);
            _context.SaveChangesAsync();
        }
    }
}
