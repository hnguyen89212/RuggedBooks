using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.Update(orderHeader);
        }
    }
}
