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

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<NormalUser> NormalUsers { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
    }
}
