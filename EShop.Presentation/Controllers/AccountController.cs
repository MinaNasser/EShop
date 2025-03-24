using EShop.Managers;
using EShop.Services;
using EShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private AccountServices accountServices;
        private RoleManager roleManager;

        public AccountController(AccountServices accountServices, RoleManager roleManager) 
        {
            this.accountServices = accountServices;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.roles=roleManager.GetRoles();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegister)
        {
            var result= await accountServices.CreateAccount(userRegister);
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
                    ViewBag.roles = roleManager.GetRoles();
                    return View();
                }
            }
            ViewBag.roles = roleManager.GetRoles();
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
                var res = await accountServices.Login(vmodel);
                if (res.Succeeded)
                {
                    var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                    if (role== "Vendor")
                    {
                        return RedirectToAction("AdminPanal", "Home");
                    }
                    return RedirectToAction("Index", "Home");

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
            await accountServices.Logout();
            return RedirectToAction("Index", "Home");
        }

    }

}

