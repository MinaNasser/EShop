using EF_Core;
using EF_Core.Models;
using EShop.Manegers;
using Microsoft.EntityFrameworkCore;


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
                Edit(existing);
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductID = productId,
                    Quantity = 1,
                    ClientId = clientId
                };
                Add(cartItem);
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
        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await GetList().Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            Edit(cartItem);
            await Commit(); 
        }


    }

}
