using AutoMapper;
using eProfessional.BLL.Auth;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;


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

        public List<UserDto> GetUsers()
        {
            var users = _userRepository.GetUsers();
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

        public UserDto GetUserByUsername(string username)
        {
            username = username.ToLowerInvariant().Trim();
            try
            {
                var user = _userRepository.GetUserByUsername(username);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user by email: Error: ", ex);
            }
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            var userExists = _userRepository.GetById(userDto.Iduser);

            if (userExists == null) return userDto;

            if (userDto.Password != null)
            {
                var b64salt = HashPwd.GetSalt();
                var b64hash = HashPwd.GetHash(userDto.Password, b64salt);
                userExists.PasswordHash = b64hash;
                userExists.PasswordSalt = b64salt;
            }

            userExists.Username = userDto.Username ?? userExists.Username;
            userExists.FirstName = userDto.FirstName ?? userExists.FirstName;
            userExists.LastName = userDto.LastName ?? userExists.LastName;
            userExists.Email = userDto.Email ?? userExists.Email;
            userExists.Phone = userDto.Phone ?? userExists.Phone;

            _mapper.Map(userDto, userExists);
            _userRepository.Save();

            UserDto updatedUser = _mapper.Map<UserDto>(userExists);

            return updatedUser;
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
