using EF_Core;
using EF_Core.Models;
using EShop.Manegers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Identity;
namespace EShop.Managers
{
    public class AccountManager:BaseManager<User>
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private VendorManager vendorManager;
        private ClientManager clientManager;
        public AccountManager(
            EShopContext context,
            UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            VendorManager _vendorManager,
            ClientManager _clientManager
            ):base(context)
        {
            userManager =_userManager;
            signInManager = _signInManager;
            vendorManager = _vendorManager;
            clientManager = _clientManager;
            
        }

        public async Task<IdentityResult> Register(UserRegisterViewModel userRegister)
        {

            var res = await userManager.CreateAsync(userRegister.ToModel(), userRegister.Password);
            if (res.Succeeded)
            {
                User user = await userManager.FindByNameAsync(userRegister.UserName);

                res = await userManager.AddToRoleAsync(user, userRegister.Role);
                return res;
            }
            return res;
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
        public async Task<User> FindByUserName(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }
        public async Task<User> FindByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AsignUserToRole(User user, string newrole)
        {
            return await userManager.AddToRoleAsync(user, newrole);
        }
        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }








    }
}
