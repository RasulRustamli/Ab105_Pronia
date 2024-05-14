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

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
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


            await _signInManager.SignInAsync(appUser,false);
           
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
           await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
