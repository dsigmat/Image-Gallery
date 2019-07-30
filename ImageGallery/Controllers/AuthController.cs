using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    public class AuthController : Controller
    {
        //сервис SignInManager, который позволяет аутентифицировать пользователя
        private SignInManager<IdentityUser> _signInManager;
        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        //вывожу форму представления авторизации
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            //сервис SignInManager, который позволяет аутентифицировать пользователя
            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            return RedirectToAction("Index", "Panel");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Gallery");
        }
    }
}