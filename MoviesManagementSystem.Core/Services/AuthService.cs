using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Dots.UserDtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.Core.Settings;
using MoviesManagementSystem.EF.Models;
using OrderManagementSystem.EF.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MoviesManagementSystem.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTTokenGenerator _jwtTokenGenerator;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;
        public AuthService(UserManager<ApplicationUser> userManager,JWTTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthModel> RegisterAsync(RegisterDto dto, string Role)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                return new AuthModel { Message = "Username is already registered!" };

            var extension = Path.GetExtension(dto.ProfileImg.FileName);

            if (!_AllowedExtensions.Contains(extension.ToLower()))
                return new AuthModel { Message = "Only .jpg and .png images are allowed." };

            if (dto.ProfileImg.Length > _MaxAllowedSize)
                return new AuthModel { Message = "Max allowed size is 10MB." };

            var imageFileName = $"{Guid.NewGuid()}{extension}";
            var imagePath = Path.Combine("wwwroot/images/users", imageFileName);
            var relativeImagePath = $"/images/users/{imageFileName}";

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.ProfileImg.CopyToAsync(stream);
            }

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ProfileImgPath = relativeImagePath,
                Age = dto.Age,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                switch (Role)
                {
                    case "NormalUser":
                        var normalUser = new NormalUser { UserId = user.Id };
                        await _unitOfWork.NormalUsers.AddAsync(normalUser);
                        break;

                    case "Admin":
                        var admin = new Admin { UserId = user.Id };
                        await _unitOfWork.Admins.AddAsync(admin);
                        break;

                    case "SuperAdmin":
                        var superAdmin = new SuperAdmin { UserId = user.Id };
                        await _unitOfWork.SuperAdmins.AddAsync(superAdmin);
                        break;

                    default:
                        return new AuthModel { Message = "Invalid role!" };
                }

                await _unitOfWork.SaveChangesAsync();
            }

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, Role);

            var jwtSecurityToken = await _jwtTokenGenerator.CreateJwtTokenAsync(user);

            return new AuthModel
            {
                UserId = user.Id,
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { Role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
            };
        }

        public async Task<AuthModel> DeleteUserAsync(string userId)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                authModel.Message = "User Not Found!";
                return authModel;
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            return new AuthModel
            {
                UserId = user.Id,
                Email = user.Email,
                IsAuthenticated = true,
                Username = user.UserName,
                Message = "User deleted successfully",
            };
        }


        public async Task<AuthModel> GetTokenAsync(LoginDto model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await _jwtTokenGenerator.CreateJwtTokenAsync(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            authModel.UserId = user.Id;

            return authModel;
        }

    }
}
