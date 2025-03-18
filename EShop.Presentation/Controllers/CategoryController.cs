using EF_Core;
using EF_Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace EShop.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private EShopContext context = new EShopContext();
        //    ...... /Categry/list
        public IActionResult List()
        {
            var list = context.Categories.ToList();

            return View("Index",list);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit( int Id,string name)
        {
            var selected = context.Categories.FirstOrDefault(i => i.Id == Id);
            return View(selected);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
            return RedirectToAction("List");
        }
        //public IActionResult testjson()
        //{
        //    return new JsonResult(new { Id = 1, Name = "Heba" });
        //}
        public IActionResult Delete(int id)
        {
            var cat = context.Categories.FirstOrDefault(i=>i.Id==id);
            if (cat != null)
            {
                context.Categories.Remove(cat);
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                return NotFound();
            }


        }
    }
}
