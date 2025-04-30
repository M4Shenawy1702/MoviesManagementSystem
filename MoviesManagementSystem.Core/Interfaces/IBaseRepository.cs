using System.Linq.Expressions;

namespace MoviesManagementSystem.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<T> GetByIdAsync<TKey>(TKey Id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> FindAsync(Expression<Func<T, bool>> match, string[] Includes = null);
        public Task<IEnumerable<T>> FindAllWithCraiteriaAsync(Expression<Func<T, bool>> Craiteria, string[] Includes = null);
        public Task<IEnumerable<T>> FindAllWithIncludesAsync(string[] Includes = null);
        public Task<T> AddAsync(T entity);
        public T Delete(T entity);
        public T Update(T entity);
    }
}