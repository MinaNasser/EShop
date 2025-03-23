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

        public async Task<IdentityResult> Register(UserRegisterViewModel userRegisterVM)
        {
            IdentityResult res = await userManager.CreateAsync(userRegisterVM.ToModel(), userRegisterVM.Password);
            if (res.Succeeded)
            {
                User user = await userManager.FindByNameAsync(userRegisterVM.UserName);
                res =await userManager.AddToRoleAsync(user,userRegisterVM.Role);
                if (userRegisterVM.Role == "Vendor")
                {
                    Vendor vendor = new Vendor
                    {
                        UserId = user.Id

                    };
                    vendorManager.Add(vendor);
                }else if(userRegisterVM.Role == "Client")
                {
                    Client client = new Client
                    {
                        UserId = user.Id
                    };
                    clientManager.Add(client);
                }
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
        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }








    }
}
