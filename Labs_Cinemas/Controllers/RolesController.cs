using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs_Cinemas.Models;
using Labs_Cinemas.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Labs_Cinemas.Controllers
{

    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId) {
            // отримуємо користувача
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null) {
                //список ролей користувача
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            if (ModelState.IsValid) {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    return RedirectToAction("UserList");
                } else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit1(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit1(EditUserViewModel model) {
            if (ModelState.IsValid) {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null) {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("UserList");
                    } else {
                        foreach (var error in result.Errors) {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("UserList");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles) {
            // отримуємо користувача
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null) {
                // список ролей користувача
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // список ролей, які було додано
                var addedRoles = roles.Except(userRoles);
                // список ролей, які було видалено
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
        public async Task<IActionResult> ChangePassword(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {
            if (ModelState.IsValid) {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null) {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded) {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("UserList");
                    } else {
                        foreach (var error in result.Errors) {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                } else {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
