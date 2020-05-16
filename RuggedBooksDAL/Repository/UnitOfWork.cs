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

        public ICategoryRepository Category { get; private set; }

        public IStoredProcedureCall StoredProcedureCall { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _db = context;
            Category = new CategoryRepository(_db);
            StoredProcedureCall = new StoredProcedureCall(_db);
        }

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
