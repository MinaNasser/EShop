using EF_Core;
using EF_Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EShop.Manegers
{
    public class OrderManager : BaseManager<Order>
    {
        public OrderManager(EShopContext _eShop) : base(_eShop)
        {

        }
        
    }

}
