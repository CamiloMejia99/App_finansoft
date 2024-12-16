using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace FNTC.Finansoft.Accounting.DTO.Interfaces
{
    public abstract class GenericRepository<C, T> :
     IGenericRepository<T>
        where T : class
        where C : DbContext, new()
    {

        private C _entities = new C();
        public C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }


        public virtual IQueryable<T> GetAll()
        {
            _entities.Database.Log = Console.Write;
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            //_entities.Set<T>().
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
        public void SaveChanges()
        {
            this.Save();
        }



        /// <summary>
        /// DNTO OrderBy
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IList<T> FindByExpressionOrdered(Expression<Func<T, bool>> filter,
                                            params Expression<Func<T, object>>[] orderBy)
        {
            IOrderedQueryable<T> query = _entities.Set<T>().Where(filter).OrderBy(orderBy.First());
            if (orderBy.Length >= 1)
            {
                for (int i = 0; i < orderBy.Length; i++)
                {
                    query = query.ThenBy(orderBy[i]);
                }
            }
            return query.ToList();
        }


        public void Add(ref T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindByKey(int key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find(string filterExpression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindRange(string filterExpression, string sortingExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }




    }



}
