using AutoMapper;
using MoviesManagementSystem.Core.Dots.RateDtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Models;
using System.Net;

namespace MoviesManagementSystem.Core.Services
{
    public class RateService : IRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RateDetailsDto>> GetAllRatesForMovie(int movieId)
        {
            var rate = await _unitOfWork.Rates.FindAllWithCraiteriaAsync(r => r.MovieId == movieId)
                ?? throw new NotFoundException("Rate not found.");

            return _mapper.Map<IEnumerable<RateDetailsDto>>(rate);
        }

        public async Task<RateDetailsDto> GetRateByUserAndMovie(int movieId, string userId)
        {
            var rate = await _unitOfWork.Rates.FindAsync(r => r.MovieId == movieId && r.UserId == userId)
                ?? throw new NotFoundException("Rate not found.");

            return _mapper.Map<RateDetailsDto>(rate);
        }

        public async Task<RateDetailsDto> AddRateMovie(RateDto dto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(dto.MovieId)
                ?? throw new NotFoundException("Movie not found.");

            var user = await _unitOfWork.Users.FindAsync(u => u.Id == dto.UserId)
                ?? throw new NotFoundException("User not found.");

            var existingRate = await _unitOfWork.Rates.FindAsync(r => r.MovieId == dto.MovieId && r.UserId == dto.UserId);
            if (existingRate != null)
                throw new ServiceException((int)HttpStatusCode.BadRequest, "Rate already exists. Please use the update endpoint.");

            var rate = new Rate
            {
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                Score = dto.Score,
                Date = DateTime.UtcNow
            };

            await _unitOfWork.Rates.AddAsync(rate);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RateDetailsDto>(rate);
        }

        public async Task<RateDetailsDto> EditRateMovie(RateDto dto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(dto.MovieId)
                ?? throw new NotFoundException("Movie not found.");

            var user = await _unitOfWork.Users.FindAsync(u => u.Id == dto.UserId)
                ?? throw new NotFoundException("User not found.");

            var existingRate = await _unitOfWork.Rates.FindAsync(r => r.MovieId == dto.MovieId && r.UserId == dto.UserId)
                ?? throw new NotFoundException("Rate not found.");

            existingRate.Score = dto.Score;
            existingRate.Date = DateTime.Now;

            _unitOfWork.Rates.Update(existingRate);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RateDetailsDto>(existingRate);
        }

        public async Task<string> DeleteRateAsync(int rateId)
        {
            var rate = await _unitOfWork.Rates.GetByIdAsync(rateId)
                ?? throw new NotFoundException("Rate not found.");

            _unitOfWork.Rates.Delete(rate);
            await _unitOfWork.SaveChangesAsync();

            return "Rate deleted successfully.";
        }
    }
}
