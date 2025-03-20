using EF_Core;
using EF_Core.Enums;
using EF_Core.Models;
using EShop.Managers;
using EShop.Manegers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class OrderController : Controller
{
    private OrderManager orderManager;
    private ProductManager productManager;
    private ClientManager clientManager;
    public OrderController(OrderManager _orderManager,ProductManager _productManager,ClientManager _clientManager)
    {
        this.orderManager = _orderManager;
        this.productManager = _productManager;
        this.clientManager = _clientManager;
    }

    public IActionResult Index()
    {
        var orders = orderManager.Get()
            .Include(o => o.Client)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Select(ord => ord.ToDetailsVModel())
            .ToList();

        return View(orders);
    }

    public IActionResult Details(int id)
    {
        var order = orderManager.Get()
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
            Clients = clientManager.Get().Select(c => new SelectListItem { Value = c.UserId, Text = c.User.UserName }).ToList(),
            Products = productManager.Get().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
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
                
            };

            order.TotalQuantity = order.Items.Sum(i => i.Quantity);
            order.TotalPrice = order.Items.Sum(i => i.Quantity * i.Product.Price);

            orderManager.Add(order);

            return RedirectToAction("Index");
        }

        // Refill dropdowns if model state invalid
        model.Clients = clientManager.Get().Select(c => new SelectListItem { Value = c.UserId, Text = c.User.UserName }).ToList();
        model.Products = productManager.Get().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        model.PaymentMethods = Enum.GetValues(typeof(TypeOfPayment)).Cast<TypeOfPayment>()
                              .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() }).ToList();
        model.StatusList = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                              .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() }).ToList();

        return View(model);
    }

}
