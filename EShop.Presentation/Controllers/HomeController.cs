using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ToDash()
    {
        return RedirectToAction("product", "VendorList");
    }
    public IActionResult AdminPanal()
    {
        return View();
    }

}
