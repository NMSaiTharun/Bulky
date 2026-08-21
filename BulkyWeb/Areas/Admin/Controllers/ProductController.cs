using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorage _fileStorage;
        public ProductController(IUnitOfWork unitOfWork, IFileStorage fileStorage)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
        }

        public IActionResult Index()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productList);
        }
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(a =>            
            //new SelectListItem
            //{
            //    Text = a.Name,
            //    Value = a.Id.ToString()
            //}
            //);
            //ViewBag.CategoryList = categoryList;
            ////ViewData["CategoryList"] = categoryList;
            ///
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(a =>
                  new SelectListItem
                  {
                      Text = a.Name,
                      Value = a.Id.ToString()
                  }
                ),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(a => a.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    //delete the old image
                    await _fileStorage.DeleteAsync(obj.Product.ImageUrl);
                    obj.Product.ImageUrl = await _fileStorage.SaveAsync(file);
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["Success"] = "Product created successfully!";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["Success"] = "Product updated successfully!";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {

                obj.CategoryList = _unitOfWork.Category.GetAll().Select(a =>
                  new SelectListItem
                  {
                      Text = a.Name,
                      Value = a.Id.ToString()
                  });

                return View(obj);
            }

        }    
      
        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(a => a.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {
                await _fileStorage.DeleteAsync(productToBeDeleted.ImageUrl);
                _unitOfWork.Product.Remove(productToBeDeleted);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }

        }
        #endregion
    }
}
