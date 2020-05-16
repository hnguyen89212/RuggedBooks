using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Category category)
        {
            var fetchedCategory = _db.Categories.FirstOrDefault(each => each.Id == category.Id);

            if (fetchedCategory != null)
            {
                fetchedCategory.CategoryName = category.CategoryName;
                _db.SaveChanges();
            }
        }
    }
}
