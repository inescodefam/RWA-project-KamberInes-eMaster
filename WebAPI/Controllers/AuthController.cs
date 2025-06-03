using Microsoft.AspNetCore.Mvc;
using WebAPI.Auth;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EProfessionalContext _context;

        public AuthController(IConfiguration configuration, EProfessionalContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public ActionResult<UserDto> Register(UserAuthDto userAuthDto)
        {
            try
            {
                if (_context.Users.Any(x => x.Email == userAuthDto.Email))
                    return BadRequest($"Email {userAuthDto.Email} is already in use.");

                var b64salt = HashPwd.GetSalt();
                var b64hash = HashPwd.GetHash(userAuthDto.Password, b64salt);

                var user = new User
                {
                    PasswordHash = b64hash,
                    PasswordSalt = b64salt,
                    Email = userAuthDto.Email,
                    Username = userAuthDto.Email.Split('@')[0],
                    CreatedAt = DateTime.Now,
                };

                _context.Add(user);
                _context.SaveChanges();

                return Ok(userAuthDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult Login(UserAuthDto userAuthDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                var existingUser = _context.Users.FirstOrDefault(x => x.Email == userAuthDto.Email);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                var b64hash = HashPwd.GetHash(userAuthDto.Password, existingUser.PasswordSalt);
                if (b64hash != existingUser.PasswordHash)
                    return BadRequest(genericLoginFail);

                var secureKey = _configuration["JWT:SecureKey"];

                return Ok(JwtTokenProvider.CreateToken(secureKey, 120, userAuthDto.Email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
