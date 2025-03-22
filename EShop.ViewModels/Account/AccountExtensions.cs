using EF_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Account
{
    public static class AccountExtensions
    {
        public static User ToModel(this UserRegisterViewModel userRegister)
        {
            return new User
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                PhoneNumber = userRegister.PhoneNumber
            };
        } 
    }
}
