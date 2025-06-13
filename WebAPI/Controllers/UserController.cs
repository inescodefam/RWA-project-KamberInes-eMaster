using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using WebAPI.Auth;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;

        public UserController(EProfessionalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<UserDto>> Get(int count, int start = 0)
        {
            try
            {
                var users = _context.Users.Skip(start * count).Take(count);
                var userDtos = _mapper.Map<List<UserDto>>(users);

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Iduser == id);
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPut]
        public void Put([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
                return;
            }

            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == userDto.Email);

                var b64salt = HashPwd.GetSalt();
                var b64hash = HashPwd.GetHash(userDto.Password, b64salt);
                if (user != null)
                {
                    user.Username = userDto.Username ?? user.Username;
                    user.FirstName = userDto.FirstName ?? user.FirstName;
                    user.LastName = userDto.LastName ?? user.LastName;
                    user.Email = userDto.Email;
                    user.Phone = userDto.PhoneNumber ?? user.Phone;
                    user.PasswordHash = b64hash;
                    user.PasswordSalt = b64salt;
                    _context.SaveChanges();
                }
            }
            catch
            {
                NotFound();
            }
        }

        // DELETE api/<UserController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public void Delete(UserDto userDto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == userDto.Email);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }
            catch
            {
                NotFound();
            }
        }

    }

}
