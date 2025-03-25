using EF_Core.Models;
using EShop.Manegers;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.API.Controllers
{
    [ApiController]
    [Route("api/{Controller}")]
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager;


        public CategoryController(CategoryManager _categoryManager)
        {
            categoryManager = _categoryManager;
        }
        //    ...... /Categry/list

        [Route("list")]
        public IActionResult List()
        {
            var list = categoryManager.GetList()
                .Select(c => new CategoryVM
                {
                    ID = c.Id,
                    Name = c.Name,
                    Description = c.Description

                })
                .ToList();

            return Ok(list);
        }


        [HttpPost]
        [Route("add")]
        public IActionResult Add(CategoryADDVM categoryVM)
        {
            Category cat = new()
            {
                Name = categoryVM.Name
            };

            categoryManager.Add(cat);
            return Ok(new { massage = "Successfull added" });
        }

        //[HttpGet]
        //[Route("edit")]
        //public IActionResult Edit(int Id, string name)
        //{
        //    var selected = categoryManager.Get(i => i.Id == Id).FirstOrDefault();


        //    return Ok(selected);
        //}
        [HttpPost]
        [Route("edit")]
        public IActionResult Edit( string name)
        {
            var cat = categoryManager.Get(i => i.Name == name).FirstOrDefault();
            Category cc = new()
            {
                Name = cat.Name,
                Description = cat.Description




            };
            categoryManager.Edit(cc);
            return Ok(new { massage = "Successfull Edit" });
        }
        //public IActionResult testjson()
        //{
        //    return new JsonResult(new { Id = 1, Name = "Heba" });
        //}
        public IActionResult Delete(int id)
        {
            var cat = categoryManager.Get(i => i.Id == id).FirstOrDefault();
            if (cat != null)
            {
                categoryManager.Delete(cat);
                return Ok("List");
            }
            else
            {
                return NotFound();
            }


        }
    }
}
