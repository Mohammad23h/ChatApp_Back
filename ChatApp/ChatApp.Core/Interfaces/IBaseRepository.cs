using ChatApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id, string[] includes = null);
        T GetByIdWith(int id);
        T GetByIdWith(int id, string[] includes);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll(string[] includes = null);
        T Find(Expression<Func<T, bool>> match, string[] includes = null);
        T Find1(Expression<Func<T, bool>> match, string[] includes = null);
        T FindLast(Expression<Func<T, int>> match, string[] includes = null);

        IEnumerable<T> FindAll(Expression<Func<T, bool>> match, string[] includes = null);
        IEnumerable<T> FindAllAL(Expression<Func<T, bool>> match, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, string[] includes = null);
        T Update(T entity);
        IEnumerable<T> IncludeAll(IEnumerable<T> query, string[] includes = null);
        T IncludeOne(T entity, string[] includes = null);
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
        void DeleteAll();
        void Attach(T entity);
        int Count(Expression<Func<T, bool>> match);
        int Count();
        
    }
}
