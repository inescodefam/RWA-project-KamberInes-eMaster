using AutoMapper;
using eProfessional.BLL.Auth;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;


namespace eProfessional.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public List<UserDto> GetUsers(int count, int start = 0)
        {
            var users = _userRepository.GetUsers(count, start);
            if (users == null || !users.Any())
            {
                return new List<UserDto>();
            }
            return _mapper.Map<List<UserDto>>(users);
        }

        public UserDto GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }
            return _mapper.Map<UserDto>(user);
        }

        public UserDto GetUserByEmail(string email)
        {
            email = email.ToLowerInvariant().Trim();
            try
            {
                var user = _userRepository.GetUserByEmail(email);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user by email: Error: ", ex);
            }
        }

        public bool UpdateUser(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            var userExists = _userRepository.GetById(user.Iduser);

            if (userExists == null) return false;

            if (userDto.Password != null)
            {
                var b64salt = HashPwd.GetSalt();
                var b64hash = HashPwd.GetHash(userDto.Password, b64salt);
                user.PasswordHash = b64hash;
                user.PasswordSalt = b64salt;
            }

            user.Username = userDto.Username ?? userExists.Username;
            user.FirstName = userDto.FirstName ?? userExists.FirstName;
            user.LastName = userDto.LastName ?? userExists.LastName;
            user.Email = userDto.Email ?? userExists.Email;
            user.Phone = userDto.PhoneNumber ?? userExists.Phone;

            _userRepository.Update(userExists);
            _userRepository.Save();
            return true;
        }

        public void DeleteUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null) throw new Exception("User not found");

            _userRepository.Delete(user);
            _userRepository.Save();
        }

    }
}
