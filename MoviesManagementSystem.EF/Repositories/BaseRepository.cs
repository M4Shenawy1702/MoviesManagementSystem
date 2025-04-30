using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.EF.Context;
using System.Linq.Expressions;

namespace MoviesManagementSystem.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<T> GetByIdAsync<TKey>(TKey id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T> FindAsync(Expression<Func<T, bool>> match, string[] Includes = null)
        {
            IQueryable<T> Query = _context.Set<T>().AsNoTracking();
            if (Includes != null)
                foreach (var Include in Includes)
                    Query = Query.Include(Include);

            return await Query.FirstOrDefaultAsync(match);
        }
        public async Task<IEnumerable<T>> FindAllWithCraiteriaAsync(Expression<Func<T, bool>> match, string[] Includes = null)
        {
            IQueryable<T> Query = _context.Set<T>();
            if (Includes != null)
                foreach (var Include in Includes)
                    Query = Query.Include(Include);

            return await Query.Where(match).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllWithIncludesAsync(string[] Includes = null)
        {
            IQueryable<T> Query = _context.Set<T>();
            if (Includes != null)
                foreach (var Include in Includes)
                    Query = Query.Include(Include);

            return await Query.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }
        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }
    }
}
