using Microsoft.AspNetCore.Identity;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IAuthRepository Auths { get; private set; }
        public IAdminsRepository Admins { get; private set; }
        public ISuperAdminsRepository SuperAdmins { get; private set; }
        public IUserRepository Users { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IBaseRepository<Movie> Movies { get; private set; }
        public IBaseRepository<NormalUser> NormalUsers { get; private set; }
        public IBaseRepository<Review> MovieReviews { get; private set; }
        public IBaseRepository<Payment> Payments { get; private set; }
        public IBaseRepository<Like> Likes { get; private set; }
        public IBaseRepository<Rate> Rates { get; private set; }
        public IBaseRepository<Review> Reviews { get; private set; }
        public IBaseRepository<WatchList> WatchLists { get; private set; }



        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            Users = new UserRepository(_context, _userManager);
            Genres = new GenreRepository(_context, _userManager);
            Movies = new BaseRepository<Movie>(_context);
            NormalUsers = new BaseRepository<NormalUser>(_context);
            MovieReviews = new BaseRepository<Review>(_context);
            Payments = new BaseRepository<Payment>(_context);
            Likes = new BaseRepository<Like>(_context);
            Rates = new BaseRepository<Rate>(_context);
            Reviews = new BaseRepository<Review>(_context);
            WatchLists = new BaseRepository<WatchList>(_context);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
