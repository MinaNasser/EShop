using EF_Core;
using EF_Core.Models;
using EShop.Manegers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Managers
{
    public class CartItemManager : BaseManager<CartItem>
    {
        public CartItemManager(EShopContext context) : base(context) { }

        public async Task<List<CartItem>> GetCartItemsByClientIdAsync(string clientId)
        {
            return await GetListAsync(
                c => c.ClientId == clientId,
                c => c.Product // Include Product
            );
        }

        public async Task AddProductToCartAsync(int productId, string clientId)
        {
            var existing = await FirstOrDefaultAsync(c => c.ProductID == productId && c.ClientId == clientId);

            if (existing != null)
            {
                existing.Quantity += 1;
                Edit(existing); // Use BaseManager's Edit
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductID = productId,
                    Quantity = 1,
                    ClientId = clientId
                };
                Add(cartItem); // Use BaseManager's Add
            }
        }

        public async Task<int> GetCartCountAsync(string clientId)
        {
            return await CountAsync(c => c.ClientId == clientId);
        }
        public async Task RemoveItemAsync(int cartItemId)
        {
            var item = await GetList(c => c.Id == cartItemId).FirstOrDefaultAsync();
            if (item != null)
            {
                Delete(item);
            }
        }

    }

}
