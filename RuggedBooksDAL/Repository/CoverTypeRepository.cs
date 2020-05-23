using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(CoverType coverType)
        {
            var fetchedCoverType = _db.CoverTypes.FirstOrDefault(each => each.Id == coverType.Id);

            if (fetchedCoverType != null)
            {
                fetchedCoverType.Name = coverType.Name;
            }
        }
    }
}
