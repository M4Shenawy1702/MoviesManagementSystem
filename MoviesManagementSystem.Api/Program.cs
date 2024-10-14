using BLL.VotingSystem.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Settings;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;
using MoviesManagementSystem.EF.Repositories;
using Stripe;


namespace MoviesManagementSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddTransient(typeof(IBaseRepository<>),typeof(BaseRepository<>));
            //��� �������� layer ����� ��� �� BaseRepository  � Api
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(Program)); 
            builder.Services.AddCors();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
