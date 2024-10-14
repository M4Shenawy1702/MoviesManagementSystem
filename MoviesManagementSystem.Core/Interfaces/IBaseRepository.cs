using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(int Id);
        public Task<IEnumerable<T>> GetAllAsync();
        public IEnumerable<T> GetAll();
        public Task<T> FindAsync(Expression<Func<T, bool>> match, string[] Includes = null);
        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match , string[] Includes = null);
        public Task<IEnumerable<T>> FindAllAsync(string[] Includes = null);
        public Task<T> AddAsync(T entity);
        public T Delete(T entity);
        public T Update(T entity);
    }
}