using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using RuggedBooksUtilities;

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

            //coverType = _unitOfWork.CoverType.Get(Id.GetValueOrDefault());
            var parameter = new DynamicParameters();
            parameter.Add("@Id", Id);
            coverType = _unitOfWork.StoredProcedureCall.OneRecord<CoverType>(SD.Procedure_CoverType_Get, parameter);

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
                var parameter = new DynamicParameters();
                parameter.Add("@Name", coverType.Name);

                if (coverType.Id == 0)
                {
                    //_unitOfWork.CoverType.Add(coverType);
                    _unitOfWork.StoredProcedureCall.Execute(SD.Procedure_CoverType_Create, parameter);
                }
                else
                {
                    //_unitOfWork.CoverType.Update(coverType);
                    parameter.Add("@Id", coverType.Id);
                    _unitOfWork.StoredProcedureCall.Execute(SD.Procedure_CoverType_Update, parameter);
                }
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            //var allCovertypes = _unitOfWork.CoverType.GetAll();
            var allCovertypes = _unitOfWork.StoredProcedureCall.List<CoverType>(SD.Procedure_CoverType_GetAll, null);

            return Json(new
            {
                data = allCovertypes
            });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            //CoverType coverType = _unitOfWork.CoverType.Get(Id
            var parameter = new DynamicParameters();
            parameter.Add("@Id", Id);
            CoverType coverType = _unitOfWork.StoredProcedureCall.OneRecord<CoverType>(SD.Procedure_CoverType_Get, parameter);

            if (coverType == null)
            {
                return Json(new { success = false, message = "Error removing cover type. Please try again." });
            }

            //_unitOfWork.CoverType.Remove(coverType);
            _unitOfWork.StoredProcedureCall.Execute(SD.Procedure_CoverType_Delete, parameter);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Cover type is successfully removed." });
        }

        #endregion
    }
}