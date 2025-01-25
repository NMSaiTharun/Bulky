using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
                _context = context; 
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList= _context.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Category Name cannot be same as Display Order");
            }
            //if(obj.Name.ToLower()=="test")
            //{
            //    ModelState.AddModelError("", "Category Name cannot be test");
            //}
           
            if (ModelState.IsValid)
            {
                _context.Categories.Add(obj);
                _context.SaveChanges();
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }           
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category? categoryObj = _context.Categories.Find(id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            return View(categoryObj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(obj);
                _context.SaveChanges();
                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category? categoryObj = _context.Categories.Find(id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            return View(categoryObj);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {          
            Category? categoryObj = _context.Categories.Find(id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(categoryObj);
            _context.SaveChanges();
            TempData["Success"] = "Category deleted successfully!";
            return RedirectToAction("Index");           
        }
    }
}
