﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Labs_Cinemas.Models;
using Labs_Cinemas.ViewModels;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Authorization;

namespace Labs_Cinemas.Controllers
{
   // [Route("api/[controller]")]
   // [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]

        public IActionResult Register() {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                User user = new User { Email = model.Email, UserName = model.Email };
                // додаємо користувача
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(
                    //    "ConfirmEmail",
                    //    "Account",
                    //    new { userId = user.Id, code = code },
                    //    protocol: HttpContext.Request.Scheme);
                    //EmailServices emailService = new EmailServices();
                    //await emailService.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    //return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                    //установка кукі
                    List<string> role = new List<string>() { "user" };
                    await _userManager.AddToRolesAsync(user, role);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                } else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string userId, string code) {
        //    if (userId == null || code == null) {
        //        return View("Error");
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null) {
        //        return View("Error");
        //    }
        //    var result = await _userManager.ConfirmEmailAsync(user, code);
        //    if (result.Succeeded)
        //        return RedirectToAction("Index", "Home");
        //    else
        //        return View("Error");
        //}
        [HttpGet]
        public IActionResult Login(string returnUrl = null) {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                //var user = await _userManager.FindByNameAsync(model.Email);
                //if (user != null) {
                //    // проверяем, подтвержден ли email
                //    if (!await _userManager.IsEmailConfirmedAsync(user)) {
                //        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                //        return View(model);
                //    }
                //}
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded) {
                    // перевіряємо, чи належить URL додатку
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)) {
                        return Redirect(model.ReturnUrl);
                    } else {
                        return RedirectToAction("Index", "Home");
                    }
                } else {
                    ModelState.AddModelError("", "Wrong password or login");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            // видаляємо аутентифікаційні куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}