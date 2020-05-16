using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }

        IStoredProcedureCall StoredProcedureCall { get; }
    }
}
