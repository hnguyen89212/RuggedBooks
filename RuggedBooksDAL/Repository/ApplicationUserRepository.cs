using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

    }
}
