using MoviesManagementSystem.Core.Dots.RateDtos;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IRateService
    {
        Task<RateDetailsDto> AddRateMovie(RateDto dto);
        Task<string> DeleteRateAsync(int rateId);
        Task<RateDetailsDto> EditRateMovie(RateDto dto);
        Task<IEnumerable<RateDetailsDto>> GetAllRatesForMovie(int movieId);
        Task<RateDetailsDto> GetRateByUserAndMovie(int movieId, string userId);
    }
}