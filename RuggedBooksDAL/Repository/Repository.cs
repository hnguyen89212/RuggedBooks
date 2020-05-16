using Microsoft.EntityFrameworkCore;
using RuggedBooksDAL.Data;
using RuggedBooksDAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RuggedBooksDAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _db = context;
            dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int Id)
        {
            return dbSet.Find(Id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach(var eachProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(eachProp);
                }
            }

            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var eachProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(eachProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int Id)
        {
            T entity = dbSet.Find(Id);
            Remove(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
