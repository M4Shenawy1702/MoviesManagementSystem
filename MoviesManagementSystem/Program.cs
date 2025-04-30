using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Interfaces.Services;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Middleware;
using MoviesManagementSystem.Core.Services;
using MoviesManagementSystem.Core.Settings;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;
using MoviesManagementSystem.EF.Repositories;
using MoviesManagementSystem.EF.Seeding;
using OrderManagementSystem.EF.Services;
using Stripe;
using System.Text;

namespace MoviesManagementSystem.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            RegisterServices(builder.Services,builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();


            await SeedDatabaseAsync(app);

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();


            app.Run();
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<JWTTokenGenerator>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IRateService, RateService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReviewService, Core.Services.ReviewService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    };
                });

        }


        private static async Task SeedDatabaseAsync(WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUsers.SeedSuperAdminAdminUserAsync(userManager);
        }
    }
}
