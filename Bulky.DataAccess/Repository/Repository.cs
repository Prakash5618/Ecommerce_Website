using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext db) 
        {
            _db = db;
            dbset = _db.Set<T>();
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public IEnumerable<T> GetAll(string? includeproperties=null)
        
        {
            IQueryable<T> query = dbset;
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach(var includeprop in includeproperties
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) 
                {
                    query = query.Include(includeprop);
                }
            }
                return query.ToList();
        }
        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null)
        {
            IQueryable<T> query = dbset;
           if(!string.IsNullOrEmpty(includeproperties))
            {
                foreach(var includeprop in includeproperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);  
        }
        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }
    }
}
