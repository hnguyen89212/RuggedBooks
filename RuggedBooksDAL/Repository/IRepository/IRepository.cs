using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int Id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );

        void Add(T entity);

        void Remove(int Id);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
