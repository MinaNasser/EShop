using EF_Core.Models;


namespace EShop.Services
{
    public interface ICartItemService
    {
        Task<List<CartItem>> GetCartItemsByClientIdAsync(string clientId);
        Task AddToCartAsync(int productId, string clientId);
        Task<int> GetCartCountAsync(string clientId);
        Task RemoveItemAsync(int cartItemId);

    }

}
