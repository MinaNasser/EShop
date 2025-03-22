using EShop.Managers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private AccountManager accountManager;

        public AccountController(AccountManager accountManager) 
        {
            this.accountManager = accountManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegister)
        {
            var result= await accountManager.Register(userRegister);
            if (ModelState.IsValid)
            {
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                var res = await accountManager.Login(vmodel);
                if (res.Succeeded)
                {
                    return RedirectToAction("AdminPanal", "Home");
                }
                else if (res.IsLockedOut || res.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Sorry try again Later!!!!");
                }
                else
                {
                    ModelState.AddModelError("", "Sorry Invalid Email Or User Name Or Password");
                }
                return View();
            }
            return View();



        }
        [HttpGet]

        public async Task<IActionResult> Logout()
        {
            await accountManager.Logout();
            return RedirectToAction("Index", "Home");
        }

    }

}

