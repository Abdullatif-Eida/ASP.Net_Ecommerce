using Ecommerce.Areas.Identity.Data;
using Ecommerce.Vm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RegisterController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM user)
        {
            var appUser = new AppUser
            {
                Email = user.Email,
                UserName = user.Email.ToUpper(),
            };
                  await _userManager.CreateAsync(appUser, user.Password);
            return View();
        }
        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login(RegisterVM user)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
        //    if(result.Succeeded)
        //    {
        //        return RedirectToAction("Index", "AdminHome");
        //    }
        //    else
        //    {
        //        return RedirectToAction("LoginFaild");
        //    }
        //    return View();
        //}
    }
}
