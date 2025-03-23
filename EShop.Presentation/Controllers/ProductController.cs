using EF_Core.Models;
using EShop.Manegers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace EShop.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductManager productManager;
        private readonly CategoryManager categoryManager;

        public ProductController(ProductManager _productManager, CategoryManager _categoryManager)
        {
            this.productManager = _productManager;
            this.categoryManager = _categoryManager;
        }
        //public IActionResult Index(string searchText = "", decimal price = 0,
        //     int categoryId = 0, string vendorId = "", int pageNumber = 1, int pageSize = 3)
        //{
        //    ViewData["CategoriesList"] = GetCategories();

        //    PaginationViewModel<ProductDetailsViewModel> products;

        //    if (string.IsNullOrEmpty(searchText) && price == 0 && categoryId == 0 && string.IsNullOrEmpty(vendorId))
        //    {
        //        var query = productManager.Get(null, pageSize, pageNumber)
        //            .Include(p => p.Attachments)
        //            .Include(p => p.Category)
        //            .Include(p => p.Vendor);

        //        var list = query.Select(p => new ProductDetailsViewModel
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Description = p.Description,
        //            Quantity = p.Quantity,
        //            Price = p.Price,
        //            CategoryName = p.Category != null ? p.Category.Name : "",
        //            VendorName = p.Vendor != null ? p.Vendor.User.UserName : "",
        //            CreatedAt = p.CreatedAt,
        //            Images = p.Attachments.Select(a => a.Image).ToList()
        //        }).ToList();

        //        products = new PaginationViewModel<ProductDetailsViewModel>
        //        {
        //            Data = list,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            Total = productManager.GetCount()
        //        };
        //    }
        //    else
        //    {
        //        products = productManager.Search(
        //            categoryId: categoryId,
        //            vendorId: vendorId,
        //            searchText: searchText,
        //            price: price,
        //            pageNumber: pageNumber,
        //            pageSize: pageSize
        //        );
        //    }

        //    // 🔽 Calculate PageCount
        //    decimal pageCount = Math.Ceiling((decimal)products.Total / products.PageSize);
        //    ViewData["PageCount"] = pageCount;

        //    return View(products);
        //}
        public IActionResult Index(string searchText = "", decimal price = 0,
            int categoryId = 0, string vendorId = "", int pageNumber = 1,
            int pageSize = 3)
        {
            ViewData["CategoriesList"] = GetCategories();

            var list = productManager.Search(categoryId: categoryId, vendorId: vendorId,
                searchText: searchText, price: price, pageNumber: pageNumber, pageSize: pageSize);
            return View(list);
        }
        [Authorize (Roles = "Vendor,Admin")]

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["CategoriesList"] = GetCategories();
            ViewBag.Title = "Add Product";
            return View();
        }

        [Authorize(Roles = "Vendor,Admin")]

        [HttpPost]
        public IActionResult Add(AddProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Save uploaded images
                foreach (var file in viewModel.Attachments)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    viewModel.Paths.Add($"/Images/Products/{file.FileName}");
                }
                viewModel.VendorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                productManager.Add(viewModel.ToModel());

                return RedirectToAction("Index");
            }

            ViewData["CategoriesList"] = GetCategories();
            return View(viewModel);
        }
        [Authorize(Roles = "Vendor,Admin")]

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = productManager.Get(p => p.Id == id)
                .Include(p => p.Attachments)
                .FirstOrDefault();

            if (product == null) return NotFound();

            var viewModel = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                CategoryId = product.CategoryId,
                VendorId = product.VendorId,
                CreatedAt = product.CreatedAt,
                ExistingImages = product.Attachments?.Select(a => a.Image).ToList() ?? new List<string>()
            };

            ViewData["CategoriesList"] = GetCategories();
            return View(viewModel);
        }
        [Authorize(Roles = "Vendor,Admin")]


        [HttpPost]
        public IActionResult Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = productManager.Get(p => p.Id == model.Id)
                    .Include(p => p.Attachments)
                    .FirstOrDefault();

                if (product == null) return NotFound();

                product.Name = model.Name;
                product.Description = model.Description;
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                // New Attachments
                if (model.NewAttachments != null && model.NewAttachments.Any())
                {
                    foreach (var file in model.NewAttachments)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products", file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        var attachment = new ProductAttachment
                        {
                            Image = "/Images/Products/" + file.FileName,
                            ProductId = product.Id
                        };
                        product.Attachments.Add(attachment);
                    }
                }

                productManager.Edit(product);

                return RedirectToAction("Index");
            }

            ViewData["CategoriesList"] = GetCategories();
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Vendor,Admin")]
        public IActionResult Delete(int id)
        {
            var product = productManager.Get(p => p.Id == id)
                .Include(p => p.Attachments)
                .FirstOrDefault();

            if (product == null)
                return RedirectToAction("Index");

            // Delete Images
            foreach (var attachment in product.Attachments)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products", attachment.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            productManager.Delete(product);

            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetCategories()
        {
            return categoryManager.Get()
                .Select(cat => new SelectListItem(cat.Name, cat.Id.ToString())).ToList();
        }
    }
}
