using AutoMapper;
using eProfessional.BLL.Auth;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthApiController(IConfiguration configuration, IAuthService authService, IMapper mapper)
        {
            _configuration = configuration;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public ActionResult Register(AuthApiDto userAuthDto)
        {
            try
            {
                _authService.RegisterUser(_mapper.Map<AuthDto>(userAuthDto));
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult Login(AuthApiDto userAuthDto)
        {

            var secureKey = _configuration["JWT:SecureKey"];
            var genericLoginFail = "Incorrect username or password";

            var user = _mapper.Map<AuthDto>(userAuthDto);
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(genericLoginFail);
            }
            try
            {
                var existingUserRoles = _authService.LoginUser(user);
                var token = JwtTokenProvider.CreateToken(secureKey, 120, userAuthDto.Email, existingUserRoles);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }

        }
    }
}
