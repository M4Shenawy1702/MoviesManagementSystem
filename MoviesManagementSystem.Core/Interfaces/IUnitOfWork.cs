using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Movie> Movies { get; }
        IBaseRepository<NormalUser> NormalUsers { get; }
        IBaseRepository<MovieReview> MovieReviews { get; }
        IBaseRepository<Payment> Payments { get; }
        int Complete();
    }
}
