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
    public class VendorManager:BaseManager<Vendor>
    {
        public VendorManager(EShopContext context):base(context) { }
        
            
        
    }
}
