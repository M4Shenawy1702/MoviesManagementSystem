using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.ReviewDtos;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IReviewService
    {
        Task<ReviewDetailsDto> AddReview([FromForm] ReviewDto Dto);
        Task<string> DeleteReview(int ReviewId);
        Task<ReviewDetailsDto> EditReview([FromForm] ReviewDto dto, int id);
        Task<IEnumerable<ReviewDetailsDto>> GetAllReviewsForMovie(int movieId);
        Task<ReviewDetailsDto> GetReviewsForMovie(int id);
    }
}