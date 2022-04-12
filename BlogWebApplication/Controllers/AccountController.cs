using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BlogBLL.DTO;
using BlogBLL.Interfaces;
using BlogWebApplication.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWebApplication.Controllers
{
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVm model)
        {
            if (!ModelState.IsValid)
            {                
                return View();
            }

            if (accountService.DoesTheUserValid(model.Email, model.Password))
            {
                await SetAuthenticationCookies(accountService.GetUserByEmail(model.Email));
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Login or password is invalid!");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<RegisterUserVM, RegisterUserDto>(model);

            if (!accountService.DoesTheUserExist(model.Email))
            {
                await SetAuthenticationCookies(accountService.CreateUser(modelDto));
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Login exists!");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UsersList()
        {
            var users = accountService.UsersList();

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int userId)
        {
            accountService.DeleteUser(userId);

            return RedirectToAction("UsersList", "Account");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditUserPassword()
        {
            return View();
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUserRole()
        {
            ViewBag.List = new SelectList(accountService.GetRoles());

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUserRole(EditUserRoleVM model, int userId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<EditUserRoleVM, EditUserDto>(model);

            accountService.EditUserRole(modelDto, userId);

            return RedirectToAction("UsersList", "Account");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUserPassword(EditUserPasswordVm model, int userId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<EditUserPasswordVm, EditUserDto>(model);

            accountService.EditUserPassword(modelDto, userId);

            return RedirectToAction("UsersList", "Account");
        }

        public async Task ExternalLogin()
        {
            await HttpContext.ChallengeAsync("Google", new AuthenticationProperties { RedirectUri = Url.Action("HandleExternalLogin", "Account")});
        }

        public IActionResult HandleExternalLogin()
        {
            if (!accountService.DoesTheUserExist(this.CurrentUserEmail()))
            {
                var model = new RegisterUserDto { Email = this.CurrentUserEmail(),
                    Name = this.CurrentUserName(), Password = "", Role = RoleDto.User, IsExternalAccount = true };

                accountService.CreateUser(model);
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task SetAuthenticationCookies(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}