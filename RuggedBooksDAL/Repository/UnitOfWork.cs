using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext context)
        {
            _db = context;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            StoredProcedureCall = new StoredProcedureCall(_db);
            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }

        public ICoverTypeRepository CoverType { get; private set; }

        public IProductRepository Product { get; private set; }

        public IStoredProcedureCall StoredProcedureCall { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
