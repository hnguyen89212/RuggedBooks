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
                return View(Id);
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

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCategories = _unitOfWork.Category.GetAll();
            return Json(new {
                data = allCategories
            });
        }

        #endregion
    }
}