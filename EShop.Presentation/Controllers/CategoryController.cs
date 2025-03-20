using EF_Core;
using EF_Core.Models;
using EShop.Manegers;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace EShop.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager;


        public CategoryController(CategoryManager _categoryManager)
        {
            categoryManager = _categoryManager;
        }
        //    ...... /Categry/list
        public IActionResult List()
        {
            var list = categoryManager.Get().ToList();

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
            categoryManager.Add(category);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit( int Id,string name)
        {
            var selected = categoryManager.Get(i=>i.Id==Id).FirstOrDefault();
            return View(selected);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            categoryManager.Edit(category);
            return RedirectToAction("List");
        }
        //public IActionResult testjson()
        //{
        //    return new JsonResult(new { Id = 1, Name = "Heba" });
        //}
        public IActionResult Delete(int id)
        {
            var cat = categoryManager.Get(i=>i.Id==id).FirstOrDefault();
            if (cat != null)
            {
                categoryManager.Delete(cat);
                return RedirectToAction("List");
            }
            else
            {
                return NotFound();
            }


        }
    }
}
