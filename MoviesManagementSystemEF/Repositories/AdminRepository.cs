using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.EF.Repositories
{
    public class AdminRepository :  IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        //public Task<Admin> GetById(string UserId)
        //{
        //    return _context.Admins.FirstOrDefaultAsync(x => x.UserId == UserId);
        //}
    }
}
