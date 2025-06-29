using eProfessional.BLL.Auth;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;

namespace eProfessional.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public List<string> LoginUser(AuthDto userAuthDto)
        {
            var existingUser = _authRepository.GetUserByEmail(userAuthDto.Email);

            var b64hash = HashPwd.GetHash(userAuthDto.Password, existingUser.PasswordSalt);

            if (existingUser == null || b64hash != existingUser.PasswordHash)
                throw new InvalidOperationException("User not found");

            var roles = _authRepository.GetUserRole(existingUser.Iduser);

            return roles ?? new List<string>();
        }

        public bool RegisterUser(AuthDto userAuthDto)
        {
            var user = _authRepository.GetUserByEmail(userAuthDto.Email);
            if (user != null)
            {
                throw new InvalidOperationException($"Email {userAuthDto.Email} is already in use.");
            }
            var b64salt = HashPwd.GetSalt();
            var b64hash = HashPwd.GetHash(userAuthDto.Password, b64salt);
            user = new DAL.Models.User
            {
                PasswordHash = b64hash,
                PasswordSalt = b64salt,
                Email = userAuthDto.Email,
                Username = userAuthDto.Email.Split('@')[0],
                CreatedAt = DateTime.Now,
            };
            _authRepository.AddUser(user);
            try
            {
                _authRepository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error saving user", ex);
            }
        }
    }
}
