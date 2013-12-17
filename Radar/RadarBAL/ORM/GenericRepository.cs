using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RadarBAL.ORM
{
    public class GenericRepository<T> where T : class
    {
        internal DbContext context;
        internal IDbSet<T> IDbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.IDbSet = context.Set<T>();
        }

        public virtual T GetByID(object id)
        {
            return IDbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = IDbSet;
            return query;
        }

        public IEnumerable<T> GetAll(string includeProperties)
        {
            IQueryable<T> query = IDbSet;
            return PerformInclusions(includeProperties, query);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> where,
                                   string includeProperties)
        {
            try
            {
                IQueryable<T> query = IDbSet;
                query = PerformInclusions(includeProperties, query);
                return query.Where(where);
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        public T Single(Expression<Func<T, bool>> where, string includeProperties)
        {
            try
            {
                IQueryable<T> query = IDbSet;
                query = PerformInclusions(includeProperties, query);
                return query.Single(where);
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        public T First(Expression<Func<T, bool>> where, string includeProperties)
        {
            try
            {
                IQueryable<T> query = IDbSet;
                query = PerformInclusions(includeProperties, query);
                return query.First(where);
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        public virtual void Attach(T entity)
        {
            IDbSet.Attach(entity);
        }

        public virtual T Insert(T entity)
        {
            return IDbSet.Add(entity);
        }

        public virtual void Update(T entityToUpdate)
        {
            IDbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = IDbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                IDbSet.Attach(entityToDelete);
            }
            IDbSet.Remove(entityToDelete);
        }


        //INCLUDE COMPLEX PROPERTIES = VIRTUAL KEYWORD (PRIVATE)
        private static IQueryable<T> PerformInclusions(string includeProperties, IQueryable<T> query)
        {
            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query;
        }
    }
}
