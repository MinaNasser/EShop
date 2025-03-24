using EShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.Presentation.Controllers
{
    [Authorize(Roles = "Client")]
    public class CartItemController : Controller
    {
        private readonly ICartItemService _cartService;

        public CartItemController(ICartItemService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartService.GetCartItemsByClientIdAsync(clientId);
            ViewBag.CartCount = cartItems;
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.AddToCartAsync(id, clientId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _cartService.GetCartCountAsync(clientId);
            return Json(new { count = count });
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _cartService.RemoveItemAsync(id); // هنعملها في السيرفيس
            return RedirectToAction("Index");
        }

    }
}
