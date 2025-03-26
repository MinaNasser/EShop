using EF_Core.Models;
using EShop.Managers;
using EShop.Manegers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services
{
    public class AccountServices
    {
        AccountManager accountManager;
        VendorManager vendorManager;
        ClientManager clientManager;
        IConfiguration configuration;
        public AccountServices(
            AccountManager _accountManager,
            VendorManager _vendorManager,
            ClientManager _clientManager,
            IConfiguration _configuration
            )
        {
            accountManager = _accountManager;
            vendorManager = _vendorManager;
            clientManager = _clientManager;
            configuration= _configuration;
        }

        public async Task<IdentityResult> CreateAccount(UserRegisterViewModel user)
        {
            var userRes = await accountManager.Register(user);

            if (userRes.Succeeded)
            {
                var currentUser = await accountManager.FindByUserName(user.UserName);
                if (user.Role == "Vendor")
                {
                    //Add Record In Vendor table
                    vendorManager.Add(new Vendor() { UserId = currentUser.Id });
                    return IdentityResult.Success;
                }
                else if (user.Role == "Client")
                {
                    //Add Record In Client table
                    clientManager.Add(new Client { UserId = currentUser.Id });
                    return IdentityResult.Success;
                }

            }
            return IdentityResult.Failed();
        }

        public async Task<SignInResult> Login(UserLoginViewModel user)
        {
            return await accountManager.Login(user);
        }
        public async Task<string> GenerateToken(UserLoginViewModel user)
        {
            var res = await accountManager.Login(user);
            if (res.Succeeded)
            {
                List<Claim> claims = new List<Claim>();
                var currentUser = await accountManager.FindByUserName(user.Method);
                if (currentUser == null)
                {
                    currentUser = await accountManager.FindByEmail(user.Method);
                }
                var roles = await accountManager.GetUserRoles(currentUser);
                claims.Add(new Claim(ClaimTypes.Name, currentUser.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, currentUser.Email));
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    //issuer: false,//configuration["Jwt:Issuer"],
                    //audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration["Jwt:PrivateKey"])),
                        SecurityAlgorithms.HmacSha256
                        )
                    );
                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


            }
            else if (res.IsNotAllowed)
            {
                return "NotAllowed";
            }
            else if (res.IsLockedOut)
            {
                return "LockedOut";
            }
            else
            {
                return null;
            }
        }
        public async Task Logout()
        {
            await accountManager.Logout();
        }
    }
}