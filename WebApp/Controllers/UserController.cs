using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IMapper mapper, IRoleService roleService)
        {
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
        }

        //GET: UserController
        public async Task<IActionResult> Index(int count = 50, int start = 0)
        {
            List<UserDto> users = await _userService.GetUsers(count, start);
            List<UserVM> userVMs = _mapper.Map<List<UserVM>>(users);
            return View(userVMs);
        }


        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception)
            {
                return View();

            }
        }


        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "email");
            var userEmail = emailClaim?.Value;

            UserDto user = await _userService.GetUserByEmail(userEmail);
            if (user == null)
                return NotFound();

            var viewModel = _mapper.Map<UserVM>(user);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Json(new { success = false, errors });
            }

            try
            {
                var userDto = _mapper.Map<UserDto>(model);
                var result = await _userService.UpdateUser(userDto);

                return result
                    ? Json(new { success = true, redirectUrl = Url.Action("Search", "Service") })
                    : Json(new { success = false, message = "Update failed" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var userDto = await _userService.GetUserById(id);
            if (userDto == null)
                return NotFound();

            var viewModel = _mapper.Map<UserVM>(userDto);
            return View(viewModel);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var userDto = await _userService.GetUserById(id);
                if (userDto == null)
                    return NotFound();

                await _userService.DeleteUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateRole(int userId, string roleName)
        {
            try
            {
                RoleDto roleDto = new RoleDto
                {
                    UserId = userId,
                    RoleName = roleName
                };
                var response = await _roleService.AssignRoleToUser(roleDto);

                if (!response)
                {
                    return Json(new { success = false, message = "Failed to update role" });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
