using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Dots.UserDtos;

namespace MoviesManagementSystem.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ToggleLikeAsync(int movieId, string userId);
        Task<UserInfoDto> UpdateInfoAsync(string userId, UpdateInfoDto updateInfoDto);
        Task<UserInfoDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    }
}
