using EF_Core;
using EF_Core.Models;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EShop.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private EShopContext context = new EShopContext();

        //    .... /product/index
        //    .... /product
        public IActionResult Index()
        {
            var list = context.Products.Select(prd => prd.ToDetailsVModel()).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Add()
        {


            ViewData["CategoriesList"] = GetCategories();
            //cast  

            ViewBag.Title = "Welcome";
            //no cast
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //add to db
                //.../Images/Products/xyz.png
                //
                foreach (var file in viewModel.Attachments)
                {
                    FileStream fileStream = new FileStream(
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images" ,"Products", file.FileName),
                            FileMode.Create);

                    file.CopyTo(fileStream);

                    fileStream.Position = 0;

                    //save path to database;

                    viewModel.Paths.Add($"/Images/Products/{file.FileName}");

                }

                context.Products.Add(viewModel.ToModel());
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewData["CategoriesList"] = GetCategories();
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = context.Products
                .Include(p => p.Attachments)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

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

            ViewBag.Categories = new SelectList(context.Categories, "Id", "Name", viewModel.CategoryId);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = context.Products.Include(p => p.Attachments).FirstOrDefault(p => p.Id == model.Id);
                if (product == null) return NotFound();

                product.Name = model.Name;
                product.Description = model.Description;
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                // رفع الصور الجديدة إن وُجدت
                if (model.NewAttachments != null && model.NewAttachments.Any())
                {
                    foreach (var file in model.NewAttachments)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "~/images/Products", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            // إضافة الصورة لقاعدة البيانات
                            var attachment = new ProductAttachment
                            {
                                Image = "~/images/Products/" + fileName,
                                ProductId = product.Id
                            };
                            context.ProductAttachments.Add(attachment);
                        }
                    }
                }

                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(context.Categories, "Id", "Name", model.CategoryId);
            return View(model);
        }
        private List<SelectListItem> GetCategories()
        {
            return context.Categories
            .Select(cat => new SelectListItem(cat.Name, cat.Id.ToString())).ToList();
        }

        public IActionResult Delete(int id)
        {
            var Prod = context.Products.Find(id);
            if (Prod == null)
            {
                return View("Home/Index");
            }
            context.Remove(Prod);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
