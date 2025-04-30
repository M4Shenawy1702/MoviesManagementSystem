using MoviesManagementSystem.Core.Models;

namespace MoviesManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Auths { get; }
        IAdminsRepository Admins { get; }
        ISuperAdminsRepository SuperAdmins { get; }
        IUserRepository Users { get; }
        IGenreRepository Genres { get; }
        IBaseRepository<Movie> Movies { get; }
        IBaseRepository<NormalUser> NormalUsers { get; }
        IBaseRepository<Payment> Payments { get; }
        IBaseRepository<Like> Likes { get; }
        IBaseRepository<Rate> Rates { get; }
        IBaseRepository<Review> Reviews { get; }
        IBaseRepository<WatchList> WatchLists { get; }


        Task SaveChangesAsync();
    }
}
