﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
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
        public IActionResult Index(string role, string username, int pageSize, int page, bool partial = false)
        {
            if (pageSize == 0) pageSize = 10;

            List<UserVM> users = new List<UserVM>();
            int totalUsersCount = 0;
            if (!string.IsNullOrEmpty(role) || !string.IsNullOrEmpty(username))
            {
                users = _userService.Search(role, username, pageSize, page);
                totalUsersCount = _userService.SearchTotal(role, username);
            }
            else
            {
                users = _userService.GetUsers(pageSize, page);
                totalUsersCount = _userService.GetAllUsers().Count;
            }

            var model = new UserIndexVM
            {
                Users = users,
                Username = username,
                Role = role,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalUsersCount
            };

            if (partial)
                return PartialView("_UserTablePartial", model);

            return View(model);
        }


        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
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
        public ActionResult Edit()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "email");
            var userEmail = emailClaim?.Value;

            UserVM user = _userService.GetUserByEmail(userEmail);

            if (user == null)
                return NotFound();

            var viewModel = _mapper.Map<UserVM>(user);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(UserVM model)
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
                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "email");
                var userEmail = emailClaim?.Value;

                UserVM user = _userService.GetUserByEmail(userEmail);
                if (user == null)
                    return Json(new { success = false, message = "Unauthorized" });

                UserVM verifiedUser;
                try
                {
                    verifiedUser = _userService.GetUserByEmail(model.Email);
                }
                catch
                {
                    verifiedUser = null;
                }

                UserVM exsistingUser = _userService.GetUserById(model.Iduser);

                if (verifiedUser != null && model.Email != user.Email && verifiedUser.Iduser != model.Iduser)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new { success = false, errors });
                }

                try
                {
                    verifiedUser = _userService.GetUserByUsername(model.Username);
                }
                catch
                {
                    verifiedUser = null;
                }

                if (verifiedUser != null && model.Username != user.Username && verifiedUser.Iduser != model.Iduser)
                {
                    ModelState.AddModelError("Username", "Username already in use.");
                    var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new { success = false, errors });
                }

                var result = _userService.UpdateUser(model);

                if (user.Email != model.Email)
                {
                    Task.Run(async () => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)).GetAwaiter().GetResult();
                    Response.Cookies.Delete("jwt");

                    return result
                    ? Json(new { success = true, redirectUrl = Url.Action("Login", "Auth") })
                    : Json(new { success = false, message = "Update failed" });
                }

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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var userDto = _userService.GetUserById(id);
            if (userDto == null)
                return NotFound();

            var viewModel = _mapper.Map<UserVM>(userDto);
            return View(viewModel);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var userDto = _userService.GetUserById(id);
                if (userDto == null)
                    return NotFound();

                _userService.DeleteUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult UpdateRole(int userId, string roleName)
        {
            try
            {
                RoleVM role = new RoleVM
                {
                    UserId = userId,
                    RoleName = roleName
                };
                var response = _roleService.AssignRoleToUser(role);

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
