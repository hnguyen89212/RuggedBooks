using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Company company)
        {
            // Lets EF do all the mappings.
            _db.Update(company);
        }
    }
}
