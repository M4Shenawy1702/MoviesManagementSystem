using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Category> Categories { get; private set; }
        public IBaseRepository<Movie> Movies { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
           _context = context;

            Categories = new BaseRepository<Category>(_context);
            Movies = new BaseRepository<Movie>(_context);   
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
