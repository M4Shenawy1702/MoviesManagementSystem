using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Dots.UserDtos;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterDto registerDto, string role);
        Task<AuthModel> GetTokenAsync(LoginDto loginDto);
        Task<AuthModel> DeleteUserAsync(string userId);
    }
}