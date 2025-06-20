using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get(int count, int start = 0)
        {
            try
            {
                var userDtos = await _userService.GetUsers(count, start);

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {

                var userDto = await _userService.GetUserById(id);
                return Ok(userDto);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);

                return user != null
                ? Ok(user)
                : NotFound($"User with email {email} not found");
            }
            catch
            {
                return NotFound($"User with email: {email} can not be found!");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = _userService.UpdateUser(userDto);
                return updated
                    ? Ok("User updated successfully")
                    : NotFound("User not found");
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(UserDto userDto)
        {
            try
            {
                _userService.DeleteUser(userDto);
                return Ok("User deleted successfully");


            }
            catch
            {
                return BadRequest();
            }
        }

    }

}
