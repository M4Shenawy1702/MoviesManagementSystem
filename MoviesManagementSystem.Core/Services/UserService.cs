using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Dots.UserDtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Interfaces.Services;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;
using Stripe.Checkout;
using System.Net;


namespace MoviesManagementSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<string> ToggleLikeAsync(int movieId, string userId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
            if (movie == null) throw new NotFoundException("Movie not found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found.");

            var existingLike = await _unitOfWork.Likes.FindAsync(l => l.MovieId == movieId && l.UserId == userId);
            if (existingLike != null)
            {
                _unitOfWork.Likes.Delete(existingLike);
                movie.Likes--;
                _unitOfWork.Movies.Update(movie);
                await _unitOfWork.SaveChangesAsync();
                return "Like removed.";
            }
            else
            {
                var like = new Like
                {
                    UserId = userId,
                    MovieId = movieId
                };
                movie.Likes++;
                _unitOfWork.Movies.Update(movie);
                await _unitOfWork.Likes.AddAsync(like);
                await _unitOfWork.SaveChangesAsync();
                return "Like added.";
            }
        }


        public async Task<UserInfoDto> UpdateInfoAsync(string id, UpdateInfoDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("User Not Found");

            if (dto.Email != user.Email)
                if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                    throw new ServiceException(StatusCodes.Status406NotAcceptable, "Email Already Exists");

            if (dto.UserName != user.UserName)
                if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                    throw new ServiceException(StatusCodes.Status406NotAcceptable, "Username Already Exists");

            if (dto.ProfileImg != null)
            {
                var extension = Path.GetExtension(dto.ProfileImg.FileName);

                if (!_AllowedExtensions.Contains(extension.ToLower()))
                    throw new ServiceException(StatusCodes.Status406NotAcceptable, "Only .jpg and .png images are allowed.");

                if (dto.ProfileImg.Length > _MaxAllowedSize)
                    throw new ServiceException(StatusCodes.Status406NotAcceptable, "Max allowed size is 10MB.");

                var imageFileName = $"{Guid.NewGuid()}{extension}";
                var imagePath = Path.Combine("wwwroot/images/users", imageFileName);
                var relativeImagePath = $"/images/users/{imageFileName}";

                if (!string.IsNullOrEmpty(user.ProfileImgPath))
                {
                    var oldImagePath = Path.Combine("wwwroot", user.ProfileImgPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.ProfileImg.CopyToAsync(stream);
                }

                user.ProfileImgPath = relativeImagePath;
            }

            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.Age = dto.Age;
            user.Gender = dto.Gender;

            await _unitOfWork.SaveChangesAsync();
            var roles =await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserInfoDto>(user);
            userDto.RolesName = roles;

            return userDto;
        }
        public async Task<UserInfoDto> ChangePasswordAsync(string id, ChangePasswordDto Dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("User Not Found");

            if (!await _userManager.CheckPasswordAsync(user, Dto.OldPass))
            {
                throw new ServiceException(StatusCodes.Status400BadRequest, "the old password is wrong");
            }

            var result = await _userManager.ChangePasswordAsync(user, Dto.OldPass, Dto.NewPass);
            if (!result.Succeeded)
                throw new ServiceException(StatusCodes.Status400BadRequest, $"{result.Errors}");

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new ServiceException(StatusCodes.Status400BadRequest, $"{updateResult.Errors}");

            return _mapper.Map<UserInfoDto>(user);
        }
    }
}
