using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Identity;
using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.EF.Seeding
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                UserName = "SuperAdmin",
                Email = "SuperAdmin@MovieSystem.com",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(admin.Email);

            if (user is null)
            {
                await userManager.CreateAsync(admin, "P@ssword123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}