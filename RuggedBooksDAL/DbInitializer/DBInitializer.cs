using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuggedBooksDAL.Data;
using RuggedBooksModels;
using RuggedBooksUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuggedBooksDAL.DbInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (_db.Roles.Any(r => r.Name == SD.Role_Administrator))
            {
                return;
            }

            _roleManager.CreateAsync(new IdentityRole(SD.Role_Administrator)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Individual)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "phuchai1994@gmail.com",
                Email = "phuchai1994@gmail.com",
                EmailConfirmed = true,
                Name = "Hai Nguyen"
                // Password must have lowercase, uppercase, number, special characters.
            }, "adminPASSWORD123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "phuchai1994@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(user, SD.Role_Administrator).GetAwaiter().GetResult();
        }
    }
}
