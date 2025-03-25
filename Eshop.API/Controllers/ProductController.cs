using EShop.Manegers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/{Controller}")]
    public class ProductController : ControllerBase
    {

        private ProductManager ProductManager;
        public ProductController(ProductManager pmanager)
        {
            ProductManager = pmanager;
        }

        //    .... /product/index
        //    .... /product
        [Route("index")]
        public IActionResult Index(string searchText = "", decimal price = 0,
            int categoryId = 0, string vendorId = "", int pageNumber = 1,
            int pageSize = 5)
        {

            var list = ProductManager.Search(categoryId: categoryId, vendorId: vendorId,
                searchText: searchText, price: price, pageNumber: pageNumber, pageSize: pageSize);
            return Ok(list);
        }

        [Authorize(Roles = "Vendor")]
        [Route("VendorList")]
        public IActionResult VendorList(string searchText = "", decimal price = 0,
            int categoryId = 0, int pageNumber = 1,
            int pageSize = 3)
        {

            var myID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var list = ProductManager.Search(categoryId: categoryId, vendorId: myID,
                searchText: searchText, price: price, pageNumber: pageNumber, pageSize: pageSize);
            return Ok(list);
        }


        [Authorize(Roles = "Vendor,Admin")]

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(AddProductViewModel viewModel)
        {
            viewModel.VendorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                //add to db
                //.../Images/Products/xyz.png
                //
                foreach (var file in viewModel.Attachments)
                {
                    FileStream fileStream = new FileStream(
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products", file.FileName),
                            FileMode.Create);

                    file.CopyTo(fileStream);

                    fileStream.Position = 0;

                    //save path to database;
                    viewModel.Paths.Add($"/Images/Products/{file.FileName}");

                }

                ProductManager.Add(viewModel.ToModel());


                return Ok(new { massage = "Successfull added" });
            }

            return Ok(new { massage = "Data is invaild" });
        }
    }
}
