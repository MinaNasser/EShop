using EShop.Managers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EShop.Presentation.Controllers
{
    public class RoleController : Controller
    {
        RoleManager roleManager;
        public RoleController(RoleManager roleManager)
        {
            this.roleManager = roleManager;     
        }
        [HttpGet]
        public IActionResult Add()
        {
            var list = roleManager.GetList().Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string roleName)
        {
            if (roleName.IsNullOrEmpty())
            {
                ViewBag.Invalid = 1;
                var list = roleManager.GetList().Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToList();
                return View(list);
            }
            else
            {
                var res = await roleManager.Add(roleName);
                if (res.Succeeded)
                {
                    ViewBag.Invalid = 2;
                }
                else
                {
                    ViewBag.Invalid = 1;
                }
                var list = roleManager.GetList().Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToList();
                return View(list);
            }

        }

    }
}
