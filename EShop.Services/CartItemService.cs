using EF_Core.Models;
using EShop.Managers;

namespace EShop.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly CartItemManager _cartItemManager;

        public CartItemService(CartItemManager cartItemManager)
        {
            _cartItemManager = cartItemManager;
        }

        public async Task<List<CartItem>> GetCartItemsByClientIdAsync(string clientId)
        {
            return await _cartItemManager.GetCartItemsByClientIdAsync(clientId);
        }

        public async Task AddToCartAsync(int productId, string clientId)
        {
            await _cartItemManager.AddProductToCartAsync(productId, clientId);
        }

        public async Task<int> GetCartCountAsync(string clientId)
        {
            return await _cartItemManager.GetCartCountAsync(clientId);
        }
        public async Task RemoveItemAsync(int cartItemId)
        {
            await _cartItemManager.RemoveItemAsync(cartItemId);
        }

    }
}
