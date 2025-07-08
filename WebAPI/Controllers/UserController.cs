using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<UserApiDto>> Get(int count, int start)
        {
            try
            {
                var userDtos = _userService.GetUsers(count, start);
                var userApiDtos = _mapper.Map<List<UserApiDto>>(userDtos);
                return Ok(userApiDtos);
            }
            catch (Exception ex)
            {
                return new List<UserApiDto>();
            }

        }

        [HttpGet("search")]
        public ActionResult<List<UserApiDto>> Search(string? role, string? username, int count, int start)
        {
            try
            {
                var userDtos = _userService.Search(role, username, count, start);
                var userApiDtos = _mapper.Map<List<UserApiDto>>(userDtos);
                return Ok(userApiDtos);
            }
            catch (Exception ex)
            {
                return new List<UserApiDto>();
            }
        }

        [HttpGet("search-total")]
        public ActionResult<int> Search(string? role, string? username)
        {
            try
            {
                var total = _userService.SearchTotal(role, username);

                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("all")]
        public ActionResult<List<UserDto>> Get()
        {
            try
            {
                var userDtos = _userService.GetUsers();
                var userApiDtos = _mapper.Map<List<UserApiDto>>(userDtos);
                return Ok(userApiDtos);
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

                var userDto = _userService.GetUserById(id);
                if (userDto == null)
                {
                    return NotFound($"User with id {id} not found");
                }

                var userApiDto = _mapper.Map<UserApiDto>(userDto);
                return Ok(userApiDto);

            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("email/{email}")]
        public ActionResult<UserDto> GetUserByEmail(string email)
        {
            try
            {
                var user = _userService.GetUserByEmail(email);

                return user != null
                ? Ok(_mapper.Map<UserApiDto>(user))
                : NotFound($"User with email {email} not found");
            }
            catch
            {
                return NotFound($"User with email: {email} can not be found!");
            }
        }


        [HttpGet("username/{username}")]
        public ActionResult<UserDto> GetUserByUsername(string username)
        {
            try
            {
                var user = _userService.GetUserByUsername(username);

                return user != null
                ? Ok(_mapper.Map<UserApiDto>(user))
                : NotFound($"User with email {username} not found");
            }
            catch
            {
                return NotFound($"User with email: {username} can not be found!");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserApiDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<UserDto>(userDto);
                var updated = _userService.UpdateUser(user);


                return Ok(updated);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok("User deleted successfully");

            }
            catch
            {
                return BadRequest();
            }
        }

    }

}
