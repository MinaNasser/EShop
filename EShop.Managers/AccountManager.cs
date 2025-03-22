using EF_Core;
using EF_Core.Models;
using EShop.Manegers;
using EShop.ViewModels;
using EShop.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace EShop.Managers
{
    public class AccountManager:BaseManager<User>
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        public AccountManager(
            EShopContext context,
            UserManager<User> _userManager,
            SignInManager<User> _signInManager
            ):base(context)
        {
            userManager =_userManager;
            signInManager = _signInManager;
            
        }

        public async Task<IdentityResult> Register(UserRegisterViewModel userRegisterVM)
        {
            return await userManager.CreateAsync(userRegisterVM.ToModel(), userRegisterVM.Password);
        }
        public async Task<SignInResult> Login(UserLoginViewModel userLoginVM)
        {
            var user  = await userManager.FindByEmailAsync(userLoginVM.Method);
            if (user !=null)
            {
            return await signInManager.PasswordSignInAsync(user, userLoginVM.Password, true, true);
                
            }
            return await signInManager.PasswordSignInAsync(userLoginVM.Method, userLoginVM.Password, true, true);
        }
        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }








    }
}
