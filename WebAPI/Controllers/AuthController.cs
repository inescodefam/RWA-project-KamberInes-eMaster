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
        private readonly EMasterDbContext _context;

        public AuthController(IConfiguration configuration, EMasterDbContext context)
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

                // Hash the password
                var b64salt = HashPwd.GetSalt();
                var b64hash = HashPwd.GetHash(userAuthDto.Password, b64salt);

                //var username = userAuthDto.FirstName[0] + userAuthDto.LastName;
                var user = new User
                {
                    PasswordHash = b64hash,
                    PasswordSalt = b64salt,
                    //FirstName = userAuthDto.FirstName,
                    //LastName = userAuthDto.LastName,
                    Email = userAuthDto.Email,
                    Role = "User",
                    Username = userAuthDto.Email.Split('@')[1],
                };

                // Add user and save changes to database
                _context.Add(user);
                _context.SaveChanges();

                // Update DTO Id to return it to the client
                //userAuthDto.Id = user.Iduser;

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

                // Try to get a user from database
                var existingUser = _context.Users.FirstOrDefault(x => x.Email == userAuthDto.Email);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                // Check is password hash matches
                var b64hash = HashPwd.GetHash(userAuthDto.Password, existingUser.PasswordSalt);
                if (b64hash != existingUser.PasswordHash)
                    return BadRequest(genericLoginFail);

                // Create and return JWT token
                var secureKey = _configuration["JWT:SecureKey"];

                return Ok(JwtTokenProvider.CreateToken(secureKey, 120, userAuthDto.Email, existingUser.Role));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
