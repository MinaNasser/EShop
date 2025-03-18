using EF_Core;
using EF_Core.Enums;
using EF_Core.Models;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class OrderController : Controller
{
    private readonly EShopContext context = new EShopContext();

    public IActionResult Index()
    {
        var orders = context.Orders
            .Include(o => o.Client)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Select(ord => ord.ToDetailsVModel())
            .ToList();

        return View(orders);
    }

    public IActionResult Details(int id)
    {
        var order = context.Orders
            .Include(o => o.Client)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
            return NotFound();

        var viewModel = order.ToDetailsVModel();

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Add()
    {
        var model = new AddOrderViewModel
        {
            Clients = context.Clients.Select(c => new SelectListItem { Value = c.UserId, Text = c.User.UserName }).ToList(),
            Products = context.Products.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
            PaymentMethods = Enum.GetValues(typeof(TypeOfPayment)).Cast<TypeOfPayment>()
                                  .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() }).ToList(),
            StatusList = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                                  .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Add(AddOrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var order = new Order
            {
                ClientId = model.ClientId,
                Address = model.Address,
                City = model.City,
                Date = DateTime.Now,
                PaymentMethod = Enum.Parse<TypeOfPayment>(model.PaymentMethod),
                Status = Enum.Parse<OrderStatus>(model.Status),
                Items = model.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            order.TotalQuantity = order.Items.Sum(i => i.Quantity);
            order.TotalPrice = order.Items.Sum(i => i.Quantity * context.Products.Find(i.ProductId).Price);

            context.Orders.Add(order);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Refill dropdowns if model state invalid
        model.Clients = context.Clients.Select(c => new SelectListItem { Value = c.UserId, Text = c.User.UserName }).ToList();
        model.Products = context.Products.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        model.PaymentMethods = Enum.GetValues(typeof(TypeOfPayment)).Cast<TypeOfPayment>()
                              .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() }).ToList();
        model.StatusList = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                              .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() }).ToList();

        return View(model);
    }

}
