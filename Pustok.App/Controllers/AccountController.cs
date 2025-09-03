using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.App.Models;
using Pustok.App.ViewModels;
using System.Threading.Tasks;

namespace Pustok.App.Controllers
{
    public class AccountController(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVm userLoginVm, string ReturnUrl)
        {
            if (!ModelState.IsValid)
                return View(userLoginVm);
            var user = await userManager.FindByNameAsync(userLoginVm.UsernameOrEmail);
            if (user == null)
            {
                user = await userManager.FindByEmailAsync(userLoginVm.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                    return View(userLoginVm);
                }
            }
            //var result = await userManager.CheckPasswordAsync(user, userLoginVm.Password);
            //if (!result)
            //{
            //    ModelState.AddModelError("", "Username or password is incorrect");
            //    return View(userLoginVm);
            //}
            var result = await signInManager.PasswordSignInAsync(user, userLoginVm.Password,userLoginVm.RememberMe,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is blocked.");
                return View(userLoginVm);
            }
            if (!result.Succeeded) 
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(userLoginVm);
            }

            if(ReturnUrl is null)
                return RedirectToAction("Index","Home");
            return Redirect(ReturnUrl);
        }
        public async Task<IActionResult> Register(UserRegisterVm userRegisterVm)
        {
            if (!ModelState.IsValid)
                return View(userRegisterVm);
            var user = await userManager.FindByNameAsync(userRegisterVm.Username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "This username already taken");
                return View(userRegisterVm);
            }
            user = new AppUser
            {
                UserName = userRegisterVm.Username,
                Email = userRegisterVm.Email,
                Fullname = userRegisterVm.Fullname
            };
            var result = await userManager.CreateAsync(user, userRegisterVm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userRegisterVm);
            }
            //add role
            await userManager.AddToRoleAsync(user, "Member");
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (!await roleManager.RoleExistsAsync("Member"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            }
            return Content("Created.");
        }
        [Authorize(Roles ="Member")]
        public async Task<IActionResult> UserProfile()
        {
            UserProfileVm userProfileVm = new UserProfileVm();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            userProfileVm.userUpdateProfileVM = new UserUpdateProfileVM
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email
            };
            return View(userProfileVm);
        }
    }
}
