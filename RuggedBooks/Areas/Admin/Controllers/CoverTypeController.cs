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
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            CoverType coverType = new CoverType();

            if (Id == null)
            {
                return View(coverType);
            }

            coverType = _unitOfWork.CoverType.Get(Id.GetValueOrDefault());

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            // Where is ModelState from? It is crafted automatically.
            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                {
                    _unitOfWork.CoverType.Add(coverType);
                }
                else
                {
                    _unitOfWork.CoverType.Update(coverType);
                }
                _unitOfWork.Save();
                //return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCovertypes = _unitOfWork.CoverType.GetAll();
            return Json(new
            {
                data = allCovertypes
            });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            CoverType coverType = _unitOfWork.CoverType.Get(Id);

            if (coverType == null)
            {
                return Json(new { success = false, message = "Error removing cover type. Please try again." });
            }

            _unitOfWork.CoverType.Remove(coverType);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Cover type is successfully removed." });
        }

        #endregion
    }
}