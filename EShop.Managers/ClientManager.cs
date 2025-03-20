using EF_Core;
using EF_Core.Models;
using EShop.Manegers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Managers
{
    public class ClientManager:BaseManager<Client>
    {
        public ClientManager(EShopContext _eShop):base(_eShop)
        {
            
        }
    }
}
