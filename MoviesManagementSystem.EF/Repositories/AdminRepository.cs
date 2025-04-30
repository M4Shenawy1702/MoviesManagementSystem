using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;

namespace MoviesManagementSystem.EF.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminsRepository
    {
        public AdminRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
