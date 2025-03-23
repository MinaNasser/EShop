using EF_Core;
using EShop.Manegers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Managers
{
    public class RoleManager:BaseManager<IdentityRole>
    {
        RoleManager<IdentityRole> roleManager;
        public RoleManager(EShopContext context , RoleManager<IdentityRole> roleManager):base(context)
        {
            this.roleManager=roleManager;
        }
        public async Task<IdentityResult> Add(string roleName)
        {
            return await roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }
        public List<SelectListItem> GetRoles() 
        {
           return GetList(r => r.Name != "Admin")
              .Select(r => new SelectListItem(r.Name, r.Name)).ToList();
        }
    }
}
