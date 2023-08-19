using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace WebApplication1.Areas.Admin.Controllers
{
   
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db,IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult Index()
        {
            List<Product> list = _UnitOfWork.product.GetAll(includeproperties:"Category").ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Upsert(int? id) 
        {
            IEnumerable<SelectListItem> selectList = _UnitOfWork.category  //to list in dropdown
              .GetAll().Select(u => new SelectListItem //able to pick some columns and can convert the type
              {
                  Text = u.Name,        
                  Value = u.Id.ToString()
              });
            // ViewBag.categorylist = selectList;
            ProductVM productvm = new() {
                product = new Product(),
                category = selectList
            };
            if(id==null || id == 0)
            { //create
               return View(productvm);
            }
            else
            { //update
                productvm.product = _UnitOfWork.product.Get(u=>u.Id == id);
                return View (productvm);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file) 
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; //it'll give wwwroot folder
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwRootPath, @"images\products"); //combining wwwroot to porductpath
                    if(!string.IsNullOrEmpty(obj.product.ImageUrl)) 
                    { 
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productpath, filename), FileMode.Create)) //complete way to say image file in destined folder
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.product.ImageUrl = @"\images\products\" + filename;
                }
                if (obj.product.Id == 0)
                {
                    _UnitOfWork.product.Add(obj.product);
                }
                else
                {
                    _UnitOfWork.product.Update(obj.product);
                }

                _UnitOfWork.Save(); 
                TempData["success"] = "New Category Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                obj.category = _UnitOfWork.category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(obj);
            }
              
        }   
       
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> list = _UnitOfWork.product.GetAll(includeproperties: "Category").ToList();
            return Json(new {data = list});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _UnitOfWork.product.Get(u=>u.Id== id);
            if(productToBeDeleted == null)
            {//
                return Json(new { success= false, message="Error While Deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _UnitOfWork.product.Remove(productToBeDeleted);
            _UnitOfWork.Save();
         
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion

        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product product = _UnitOfWork.product.Get(u => u.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteCat(int id)
        //{
        //    Product? obj = _UnitOfWork.product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _UnitOfWork.product.Remove(obj);
        //    _UnitOfWork.Save();
        //    TempData["delete"] = "Category Deleted Successfully";
        //    return RedirectToAction("Index");
        //}
    }
}
