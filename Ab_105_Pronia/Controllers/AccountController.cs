﻿using Ab_105_Pronia.Abstractions;
using Ab_105_Pronia.Helpers.Account;
using Ab_105_Pronia.Helpers.Email;
using Ab_105_Pronia.Models;
using Ab_105_Pronia.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ab_105_Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService mailService;

        public AccountController(
            UserManager<User> userManager
            ,SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,IMailService mailService )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _roleManager = roleManager;
            this.mailService = mailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid) return View();



            User appUser = new User()
            {
                UserName = registerVm.UserName,
                Name = registerVm.Name,
                Surname= registerVm.Surname,
                Email = registerVm.Email,
                
            };

            var result=await _userManager.CreateAsync(appUser,registerVm.Password);
            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }


            await _userManager.AddToRoleAsync(appUser, UserRole.Member.ToString());

            
           
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LogOut()
        {
           await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm,string? ReturnUrl=null)
        {
            if (!ModelState.IsValid) return View();


            //User user =await _userManager.FindByEmailAsync(loginVm.UserNameOrEmail);
            //if (user == null)
            //{
            //    user = await _userManager.FindByNameAsync(loginVm.UserNameOrEmail);
            //}

            User user;

            if (loginVm.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVm.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVm.UserNameOrEmail);
            }
            if(user==null)
            {
                ModelState.AddModelError("", "UsernameOrEmail ve ya Password sefdir");
                return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginVm.Password, true);
            if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden cehd edin");
                return View();
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "UsernameOrEmail ve ya Password sefdir");
                return View();
            }

            await _signInManager.SignInAsync(user, loginVm.RememberMe);

            if(ReturnUrl!=null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            //await _roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Admin"
            //});
            //await _roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Member"
            //});
            //await _roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Moderator"
            //});
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = item.ToString()
                });
            }
            return Ok();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVm forgotPasswordVm)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(forgotPasswordVm.Email);
            if (user is null) return NotFound();
            //http://localhost:5105/account/ResetPassword?userId={userid}&token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token },HttpContext.Request.Scheme);
            await mailService.SendEmailAsync(new MailRequest()
            {
                Subject="Reset Password",
                ToEmail=user.Email,
                Body=$"<a href='{link}'>Reset password</a>"
            });
            Console.WriteLine(link);
            return RedirectToAction("Login");




        }
        public async Task<IActionResult> ResetPassword(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null) return NotFound();
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm vm,string userId, string token)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, token, vm.Password);



            return RedirectToAction("Login");

        }

    }
}
