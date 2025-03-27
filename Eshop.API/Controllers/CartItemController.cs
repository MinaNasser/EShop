using EShop.Services;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.API.Controllers
{
    [Authorize(Roles = "Client")]
    [ApiController]
    [Route("api/cart")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartService;

        public CartItemController(ICartItemService cartService)
        {
            _cartService = cartService;
        }

        // Get Cart Items
        [HttpGet("list")]
        public async Task<IActionResult> GetCartItems()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientId == null)
                return Unauthorized("User is not authenticated.");

            var cartItems = await _cartService.GetCartItemsByClientIdAsync(clientId);

            var cartItemDTOs = cartItems.Select(c => new CartItemViewModel
            {
                Id = c.Id,
                ProductId = c.ProductID,
                ProductName = c.Product?.Name
            });

            return Ok(cartItemDTOs);
        }

        // Add to Cart
        [HttpPost("add/{id}")]
        public async Task<IActionResult> AddToCart(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.AddToCartAsync(id, clientId);
            return Ok(new { message = "Successfully added" });
        }

        // Get Cart Count
        [HttpGet("count")]

        public async Task<IActionResult> GetCartCount()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _cartService.GetCartCountAsync(clientId);
            return Ok(new { count });
        }

        // Remove Item from Cart
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            await _cartService.RemoveItemAsync(id);
            return Ok(new { message = "Item removed" });
        }

        // Increase Quantity
        [HttpPut("increase/{id}")]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            await _cartService.IncreaseQuantityAsync(id);
            return Ok(new { message = "Quantity increased" });
        }

        // Decrease Quantity
        [HttpPut("decrease/{id}")]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            await _cartService.DecreaseQuantityAsync(id);
            return Ok(new { message = "Quantity decreased" });
        }
    }
}
