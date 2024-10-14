using MoviesManagementSystem.Core.Dots;
namespace MoviesManagementSystem.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthModel> RegisterAsync(RegisterDto Dto,string Role);
        Task<AuthModel> GetTokenAsync(TokenRequestDto model);
        Task<AuthModel> DeleteAsync(string UserId, string Role);
        Task<string> AddRoleAsync(AddRoleModel model);

    }
}