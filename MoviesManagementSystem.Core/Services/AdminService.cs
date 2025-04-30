using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Dots.UserDtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserInfoDto> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("User Not Found");


            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserInfoDto>(user);
            userDto.RolesName = roles;

            return userDto;
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserInfoDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserInfoDto
                {
                    Id = user.Id,
                    ProfileImgPath = user.ProfileImgPath,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Age = user.Age,
                    RolesName = roles
                };
                userDtos.Add(userDto);
            }

            return userDtos;
        }
    }
}
