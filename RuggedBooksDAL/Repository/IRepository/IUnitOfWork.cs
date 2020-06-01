using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }

        ICoverTypeRepository CoverType { get; }

        IProductRepository Product { get; }

        IStoredProcedureCall StoredProcedureCall { get; }

        ICompanyRepository Company { get; }

        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
