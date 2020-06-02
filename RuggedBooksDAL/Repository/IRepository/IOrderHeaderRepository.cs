using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
