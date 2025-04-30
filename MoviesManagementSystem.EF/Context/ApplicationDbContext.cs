using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;


namespace MoviesManagementSystem.EF.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<NormalUser> NormalUsers { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<Like> Likes { get; set; }

    }
}
