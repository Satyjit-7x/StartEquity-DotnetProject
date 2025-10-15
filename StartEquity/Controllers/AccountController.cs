using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StartEquity.Models;
using StartEquity.Services;
using Microsoft.AspNetCore.Identity;

namespace StartEquity.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _authService.Login(model.Email, model.Password, model.RememberMe);
                if (result.Succeeded)
                {
                    var user = _userManager.FindByEmailAsync(model.Email).Result;
                    if (user.Role == "Investor")
                    {
                        return RedirectToAction("Index", "Investor");
                    }
                    else if (user.Role == "Owner")
                    {
                        return RedirectToAction("Index", "Company");
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Register(model.Email, model.Password, model.FullName, model.Role);
                if (user != null)
                {
                    if (model.Role == "Investor")
                    {
                        _authService.CreateInvestorProfile(user.Id);
                    }
                    else if (model.Role == "Owner")
                    {
                    }

                    _authService.Login(model.Email, model.Password, false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}

