using EShop.Services;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.API.Controllers
{
    [Authorize(Roles = "Client")]
    [ApiController]
    [Route("api/{controller}")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartService;

        public CartItemController(ICartItemService cartService)
        {
            _cartService = cartService;
        }
        //[Route("add")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var cartItems = await _cartService.GetCartItemsByClientIdAsync(clientId);

            var cartItemDTOs = cartItems.Select(c => new CartItemViewModel
            {
                Id = c.Id,
                ProductId = c.ProductID,
                ProductName = c.Product?.Name
            });

            return Ok(cartItemDTOs);
        }


        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.AddToCartAsync(id, clientId);
            return Ok(new { massage = "Successfull added" });
        }
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _cartService.GetCartCountAsync(clientId);
            return Ok(new { count = count });
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _cartService.RemoveItemAsync(id); // هنعملها في السيرفيس
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            await _cartService.IncreaseQuantityAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            await _cartService.DecreaseQuantityAsync(id);
            return Ok();
        }


    }
}
