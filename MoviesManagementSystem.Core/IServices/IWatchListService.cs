using MoviesManagementSystem.Core.Dots.WatchListDtos;
using MoviesManagementSystem.Core.Models;

namespace MoviesManagementSystem.Core.Interfaces.Services
{
    public interface IWatchListService
    {
        Task<WatchListDetailsDto> GetWatchlistByIdAsync(int watchlistId);
        Task<IEnumerable<WatchListDetailsDto>> GetAllWatchlistsAsync();
        Task<WatchListDetailsDto> CreateWatchListAsync(CreateWatchListDto dto, string userId);
        Task<WatchListDetailsDto> UpdateWatchListAsync(UpdateWatchListDto dto, int watchlistId);
        Task<string> DeleteWatchListAsync(UpdateWatchListDto dto, int watchlistId);
        Task<WatchListDetailsDto?> AddMovieToWatchlistAsync(AddMovieToWatchlistDto dto, int watchlistId);
    }
}
    