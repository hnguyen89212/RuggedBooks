using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;

namespace RuggedBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            Company category = new Company();

            // Id is null when we create/add a new cateogry
            if (Id == null)
            {
                return View(category);
            }

            // Otherwise, we are trying to update an existing? category
            // For an integer, the default value is 0.
            category = _unitOfWork.Company.Get(Id.GetValueOrDefault());

            // The Id does not associate with any category.
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Where does the Company parameter come from? It is from the Upsert view above.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company category)
        {
            // Where is ModelState from? It is crafted automatically.
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _unitOfWork.Company.Add(category);
                }
                else
                {
                    _unitOfWork.Company.Update(category);
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
            var allCategories = _unitOfWork.Company.GetAll();
            return Json(new
            {
                data = allCategories
            });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            Company category = _unitOfWork.Company.Get(Id);

            if (category == null)
            {
                return Json(new { success = false, message = "Error removing category. Please try again." });
            }

            _unitOfWork.Company.Remove(category);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company is successfully removed." });
        }

        #endregion
    }
}