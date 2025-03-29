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

        // استخراج ClientId في كل استدعاء للراوتر
        private string? ClientId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// جلب جميع عناصر السلة الخاصة بالمستخدم
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                if (string.IsNullOrEmpty(ClientId))
                    return Unauthorized(new { message = "User is not authenticated." });

                var cartItems = await _cartService.GetCartItemsByClientIdAsync(ClientId);

                var cartItemDTOs = cartItems.Select(c => new CartItemViewModel
                {
                    Id = c.Id,
                    ProductId = c.ProductID,
                    ProductName = c.Product?.Name,
                    Quantity = c.Quantity,
                    SupPrice = c.Product?.Price ?? 0,
                    ProductImage = c.Product?.Attachments?.FirstOrDefault()?.Image ?? "/images/default-product.png"
                });

                return Ok(cartItemDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        /// <summary>
        /// إضافة منتج إلى السلة
        /// </summary>
        [HttpPost("add/{id}")]
        public async Task<IActionResult> AddToCart(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid product ID." });

            try
            {
                if (string.IsNullOrEmpty(ClientId))
                    return Unauthorized(new { message = "User is not authenticated." });

                await _cartService.AddToCartAsync(id, ClientId);
                return Ok(new { message = "Successfully added to cart" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add item.", error = ex.Message });
            }
        }

        /// <summary>
        /// جلب عدد المنتجات داخل السلة
        /// </summary>
        [HttpGet("count")]
        public async Task<IActionResult> GetCartCount()
        {
            try
            {
                if (string.IsNullOrEmpty(ClientId))
                    return Unauthorized(new { message = "User is not authenticated." });

                var count = await _cartService.GetCartCountAsync(ClientId);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get cart count.", error = ex.Message });
            }
        }

        /// <summary>
        /// إزالة منتج من السلة
        /// </summary>
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid item ID." });

            try
            {
                if (string.IsNullOrEmpty(ClientId))
                    return Unauthorized(new { message = "User is not authenticated." });

                await _cartService.RemoveItemAsync(id);
                return Ok(new { message = "Item removed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to remove item.", error = ex.Message });
            }
        }

        /// <summary>
        /// زيادة الكمية لمنتج معين داخل السلة
        /// </summary>
        [HttpPut("increase/{id}")]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid item ID." });

            try
            {
                if (string.IsNullOrEmpty(ClientId))
                    return Unauthorized(new { message = "User is not authenticated." });

                await _cartService.IncreaseQuantityAsync(id);
                return Ok(new { message = "Quantity increased successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to increase quantity.", error = ex.Message });
            }
        }

        /// <summary>
        /// تقليل الكمية لمنتج معين داخل السلة
        /// </summary>
        [HttpPut("decrease/{id}")]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid item ID." });

            try
            {
                if (string.IsNullOrEmpty(ClientId))
                    return Unauthorized(new { message = "User is not authenticated." });

                await _cartService.DecreaseQuantityAsync(id);
                return Ok(new { message = "Quantity decreased successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to decrease quantity.", error = ex.Message });
            }
        }
    }
}
