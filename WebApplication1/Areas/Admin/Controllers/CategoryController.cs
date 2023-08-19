using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork db)
        {
            _unitofwork = db;
        }
        public IActionResult Index()
        {
            List<Category> list = _unitofwork.category.GetAll().ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the name");
            }
            if (obj.Name.ToLower() == "hawkins")
            {
                ModelState.AddModelError("Name", "Hawkins is Invalid Entry");
            }
            if (ModelState.IsValid)
            {
                _unitofwork.category.Add(obj);
                _unitofwork.Save();
                TempData["success"] = "New Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categorydb = _unitofwork.category.Get(u => u.Id == id);  // Category categorydb1 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();

            if (categorydb == null)
            {
                return NotFound();
            }
            return View(categorydb);

        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.category.Update(obj);
                _unitofwork.Save();
                TempData["update"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? Deletedb = _unitofwork.category.Get(u => u.Id == id);
            if (Deletedb == null)
            {
                return NotFound();
            }
            return View(Deletedb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCat(int id)
        {
            Category? obj = _unitofwork.category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.category.Remove(obj);
            _unitofwork.Save();
            TempData["delete"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
