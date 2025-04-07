using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly EMasterDbContext _context;

        public UserController(EMasterDbContext context)
        {
            _context = context;
        }
        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<User>> Get(int start = 0, int count = 10)
        {
            try
            {
                return Ok(_context.Users.Skip(start * count).Take(count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            try
            {
                return Ok(_context.Users.FirstOrDefault(x => x.Iduser == id)?.Username ?? "User not found");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            if (_context.Users.Any(x => x.Username == userDto.Username) || _context.Users.Any(x => x.Email == userDto.Email))
            {
                BadRequest("Username already exists");
            }

            try
            {
                var user = new User
                {
                    Username = userDto.Username,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.PhoneNumber,
                    PasswordHash = userDto.Password,
                    PasswordSalt = userDto.Password,
                    Address = userDto.Address,
                    CreatedAt = DateTime.Now,
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch
            {
                NotFound();
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Iduser == id);
                if (user != null)
                {
                    user.Username = userDto.Username;
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.Email = userDto.Email;
                    user.Phone = userDto.PhoneNumber;
                    user.Address = userDto.Address;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NotFound();
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Iduser == id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NotFound();
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet("admin-resource")]
        //public IActionResult GetAdminResource()
        //{
        //    return Ok("This is a protected admin resource.");
        //}
    }

}
