using RuggedBooksModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
