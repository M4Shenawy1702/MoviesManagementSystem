using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.ReviewDtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Models;

namespace MoviesManagementSystem.Core.Services
{
    public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper) : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<ReviewDetailsDto>> GetAllReviewsForMovie(int movieId)
        {
            var rate = await _unitOfWork.Reviews.FindAllWithCraiteriaAsync(r => r.MovieId == movieId)
                ?? throw new NotFoundException("Review not found.");

            return _mapper.Map<IEnumerable<ReviewDetailsDto>>(rate);
        }
        public async Task<ReviewDetailsDto> GetReviewsForMovie(int id)
        {
            var rate = await _unitOfWork.Reviews.GetByIdAsync(id)
                ?? throw new NotFoundException("Review not found.");

            return _mapper.Map<ReviewDetailsDto>(rate);
        }

        public async Task<ReviewDetailsDto> AddReview([FromForm] ReviewDto dto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(dto.MovieId);
            if (movie == null) throw new NotFoundException("Movie not found");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null) throw new NotFoundException("Movie not found");

            var review = _mapper.Map<Review>(dto);

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewDetailsDto>(review);
        }
        public async Task<ReviewDetailsDto> EditReview([FromForm] ReviewDto dto, int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null) throw new NotFoundException("Review not found");

            review.UserId = dto.UserId;
            review.MovieId = dto.MovieId;
            review.Content = dto.Content;
            review.Date = DateTime.UtcNow;

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewDetailsDto>(review);

        }
        [HttpDelete("DeleteReview/{ReviewId}")]
        public async Task<string> DeleteReview(int ReviewId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(ReviewId);
            if (review == null) throw new NotFoundException("Review not found");

            _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.SaveChangesAsync();
            return "Deleted successfully";
        }
    }
}
