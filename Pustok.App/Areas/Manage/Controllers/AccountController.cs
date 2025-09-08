using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.App.Areas.Manage.ViewModels;
using Pustok.App.Models;
using System.Security.Claims;

namespace Pustok.App.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVm adminLoginVm)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(adminLoginVm);
            }
            var user = await userManager.FindByNameAsync(adminLoginVm.Username);
            if(user == null || !await userManager.CheckPasswordAsync(user, adminLoginVm.Password))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(adminLoginVm);
            }
            if (await userManager.IsInRoleAsync(user,"Member"))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(adminLoginVm);
            }
            var result = await userManager.CheckPasswordAsync(user, adminLoginVm.Password);
            if(!result)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(adminLoginVm);
            }

            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index","Dashboard");
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser appUser = new AppUser
            {
                Fullname = "Admin",
                UserName = "admin",
                Email = "admin@gmail.com"
            };
            var result = await userManager.CreateAsync(appUser, "_Admin123");
            await userManager.AddToRoleAsync(appUser, "Admin");
            return Json(result);
        }
        public async Task<IActionResult> UserProfile()
        {
            //if (!HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login");
            //}
            //var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userId = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
            return Json(userId);
        }
    }
}
