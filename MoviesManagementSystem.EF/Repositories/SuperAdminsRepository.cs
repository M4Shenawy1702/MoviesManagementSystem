using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;

namespace MoviesManagementSystem.EF.Repositories
{
    public class SuperAdminsRepository : BaseRepository<SuperAdmin>, ISuperAdminsRepository
    {
        public SuperAdminsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
