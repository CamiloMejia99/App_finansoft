using System;
using System.Linq;
using System.Linq.Expressions;

namespace FNTC.Finansoft.Accounting.DTO.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();


        //void Add(ref T entity);
        //void Delete(int key);
        //IQueryable<T> FindByKey(int key);
        //IEnumerable<T> FindAll();
        //IEnumerable<T> Find(string filterExpression);
        //IEnumerable<T> FindRange(string filterExpression, string sortingExpression, int startIndex, int count);

        //void SaveChanges();
    }
}
