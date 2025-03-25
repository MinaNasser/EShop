using EF_Core.Models;
using EShop.Managers;


namespace EShop.Services
{
    public interface ICartItemService
    {
        Task<List<CartItem>> GetCartItemsByClientIdAsync(string clientId);
        Task AddToCartAsync(int productId, string clientId);
        Task<int> GetCartCountAsync(string clientId);
        Task RemoveItemAsync(int cartItemId);
        Task IncreaseQuantityAsync(int cartItemId);
        Task DecreaseQuantityAsync(int cartItemId);


    }

}
