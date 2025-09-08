using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Pustok.App.Models;
using Pustok.App.ViewModels;

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
            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError("", "You cannot enter this page");
                return View(userLoginVm);
            }
            //var result = await userManager.CheckPasswordAsync(user, userLoginVm.Password);
            //if (!result)
            //{
            //    ModelState.AddModelError("", "Username or password is incorrect");
            //    return View(userLoginVm);
            //}
            var result = await signInManager.PasswordSignInAsync(user, userLoginVm.Password, userLoginVm.RememberMe, true);
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email address.");
                return View(userLoginVm);
            }
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

            if (ReturnUrl is null)
                return RedirectToAction("Index", "Home");
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
            //var telegram = new TelegramService();
            //await telegram.SendMessageAsync($"New user created: {user.Fullname}", null);

            //send email confirmation
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", 
    new { email = user.Email, token = token }, 
    Request.Scheme);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("allupproje@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Confirm your email address";
            using (StreamReader reader = new StreamReader("wwwroot/templates/emailConfirmTemplate.html"))
            {
                string html = reader.ReadToEnd();
                html = html.Replace("{{confirmationLink}}", confirmationLink);
                html = html.Replace("{{name}}", user.UserName);
                email.Body = new TextPart(TextFormat.Html) { Text = html };
            }

            //send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("allupproje@gmail.com", "pqrf fcbl fmvy kkzf");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (email == null || token == null)
                return NotFound();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            if(!await userManager.VerifyUserTokenAsync(user,userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation",token))
            {
                return Content("False");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            await userManager.UpdateSecurityStampAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            return BadRequest("Email confirmation failed.");
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
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UserProfile(string tab = "dashboard")
        {
            ViewBag.Tab = tab;
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
        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UserProfile(UserUpdateProfileVM userUpdateProfileVM)
        {
            ViewBag.Tab = "profile";
            if (!ModelState.IsValid) 
                return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if(user is null)
                return RedirectToAction("Login","Account");
            
            var isExistUserName = await userManager.FindByNameAsync(userUpdateProfileVM.Username);
            if(isExistUserName is not null && isExistUserName.Id != user.Id)
            {
                ModelState.AddModelError("Username", "This username already taken");
                return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
            }
            var isExistEmail = await userManager.FindByEmailAsync(userUpdateProfileVM.Email);
            if (isExistEmail is not null && isExistEmail.Id != user.Id)
            {
                ModelState.AddModelError("Email", "This email already taken");
                return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
            }
            user.Fullname = userUpdateProfileVM.Fullname;
            user.UserName = userUpdateProfileVM.Username;
            user.Email = userUpdateProfileVM.Email;
            if (!string.IsNullOrWhiteSpace(userUpdateProfileVM.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(userUpdateProfileVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is required");
                    return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
                }
                var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, userUpdateProfileVM.CurrentPassword);
                if (!isCurrentPasswordValid)
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is incorrect");
                    return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
                }
                if(userUpdateProfileVM.NewPassword != userUpdateProfileVM.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password not match");
                    return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
                }
                var isSamePassword = await userManager.CheckPasswordAsync(user, userUpdateProfileVM.NewPassword);
                if (isSamePassword)
                {
                    ModelState.AddModelError("NewPassword", "New password cannot be same as current password");
                    return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
                }
                var result = await userManager.ChangePasswordAsync(user, userUpdateProfileVM.CurrentPassword, userUpdateProfileVM.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
                }
               
            }
            var identity = await userManager.UpdateAsync(user);
            if (!identity.Succeeded)
            {
                foreach (var error in identity.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(new UserProfileVm { userUpdateProfileVM = userUpdateProfileVM });
            }
            await signInManager.SignInAsync(user, true);
            return RedirectToAction("UserProfile", "Account", new {tab = "profile" });
        }
    } 
}
