using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company company);
    }
}
