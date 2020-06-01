using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;

namespace RuggedBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext context = null)
        {
            _unitOfWork = unitOfWork;
            if (context != null)
            {
                _db = context;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allApplicationUsers = _unitOfWork.ApplicationUser.GetAll(includeProperties:"Company");
            //var allApplicationUsers = _db.ApplicationUsers.Include(u => u.Company).ToList(); // ApplicationDbContext approach.

            // Since we set the Role in ApplicationUser to be not mapped. For rendering an user's role in Razor,
            // We need to perform join table mapping so each user's role is initialized.
            
            var userRoles = _db.UserRoles.ToList(); // join table between users-roles

            var roles = _db.Roles.ToList();

            foreach(var user in allApplicationUsers)
            {
                var roleId = userRoles.FirstOrDefault(r => r.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                // To prevent the Company attribute from being null, throwing Exeception.
                if (user.Company == null)
                {
                    user.Company = new Company() {
                        Name = ""
                    };
                }
            }

            return Json(new
            {
                data = allApplicationUsers
            });
        }

        #endregion
    }
}