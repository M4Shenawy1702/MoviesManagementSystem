using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Identity;

namespace MoviesManagementSystem.EF.Seeding
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
                await roleManager.CreateAsync(new IdentityRole(AppRoles.NormalUser));
                await roleManager.CreateAsync(new IdentityRole(AppRoles.SuperAdmin));
            }
        }
    }
}