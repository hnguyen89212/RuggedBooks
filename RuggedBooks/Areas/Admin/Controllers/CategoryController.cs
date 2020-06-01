using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using RuggedBooksUtilities;

namespace RuggedBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Administrator)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            Category category = new Category();

            // Id is null when we create/add a new cateogry
            if (Id == null)
            {
                return View(category);
            }

            // Otherwise, we are trying to update an existing? category
            // For an integer, the default value is 0.
            category = _unitOfWork.Category.Get(Id.GetValueOrDefault());

            // The Id does not associate with any category.
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Where does the Category parameter come from? It is from the Upsert view above.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            // Where is ModelState from? It is crafted automatically.
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _unitOfWork.Category.Add(category);
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                }
                _unitOfWork.Save();
                //return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCategories = _unitOfWork.Category.GetAll();
            return Json(new
            {
                data = allCategories
            });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            Category category = _unitOfWork.Category.Get(Id);

            if (category == null)
            {
                return Json(new { success = false, message = "Error removing category. Please try again." });
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Category is successfully removed." });
        }

        #endregion
    }
}