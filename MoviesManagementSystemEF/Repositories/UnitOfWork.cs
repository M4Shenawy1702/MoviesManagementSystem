using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;
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
        public IBaseRepository<NormalUser> NormalUsers { get; private set; }
        public IBaseRepository<MovieReview> MovieReviews { get; private set; }
        public IBaseRepository<Payment> Payments { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Categories = new BaseRepository<Category>(_context);
            Movies = new BaseRepository<Movie>(_context);
            NormalUsers = new BaseRepository<NormalUser>(_context);
            MovieReviews = new BaseRepository<MovieReview>(_context);
            Payments = new BaseRepository<Payment>(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }       
        //public async Task<Object> CompleteAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
