using EF_Core;
using EF_Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EShop.Manegers
{
    public class CategoryManager : BaseManager<Category>
    {
        public CategoryManager(EShopContext _eShop) : base(_eShop)
        {

        }

        
    }

}
