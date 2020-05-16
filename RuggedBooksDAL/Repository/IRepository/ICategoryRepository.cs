using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
