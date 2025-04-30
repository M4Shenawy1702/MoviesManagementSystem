using AutoMapper;
using MoviesManagementSystem.Core.Dots.WatchListDtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Interfaces.Services;
using MoviesManagementSystem.Core.Models;
using System.Net;

namespace MoviesManagementSystem.Services
{
    public class WatchListService : IWatchListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WatchListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WatchListDetailsDto> GetWatchlistByIdAsync(int watchlistId)
        {
            var watchlist = await _unitOfWork.WatchLists.GetByIdAsync(watchlistId);
            if (watchlist == null)
                throw new NotFoundException("Watchlist not found.");

            return _mapper.Map<WatchListDetailsDto>(watchlist);
        }

        public async Task<IEnumerable<WatchListDetailsDto>> GetAllWatchlistsAsync()
        {
            var watchlists = await _unitOfWork.WatchLists.GetAllAsync();
            return _mapper.Map<IEnumerable<WatchListDetailsDto>>(watchlists);
        }

        public async Task<WatchListDetailsDto> CreateWatchListAsync(CreateWatchListDto dto, string userId)
        {
            if (dto.UserId != userId)
                throw new ServiceException((int)HttpStatusCode.BadRequest, "User ID mismatch.");

            var existing = await _unitOfWork.WatchLists.FindAsync(w => w.Name == dto.Name && w.UserId == userId);
            if (existing != null)
                throw new ServiceException((int)HttpStatusCode.Conflict, "A watchlist with this name already exists.");

            var newWatchList = new WatchList
            {
                Name = dto.Name,
                AddedAt = DateTime.UtcNow,
                UserId = userId,
            };

            await _unitOfWork.WatchLists.AddAsync(newWatchList);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WatchListDetailsDto>(newWatchList);
        }

        public async Task<WatchListDetailsDto> UpdateWatchListAsync(UpdateWatchListDto dto, int watchlistId)
        {
            var watchList = await _unitOfWork.WatchLists.FindAsync(w => w.Id == watchlistId && w.UserId == dto.UserId);
            if (watchList == null)
                throw new NotFoundException("Watchlist not found.");

            if (watchList.Name == dto.Name)
                throw new ServiceException((int)HttpStatusCode.BadRequest, "A watchlist with this name already exists.");

            watchList.Name = dto.Name;

            _unitOfWork.WatchLists.Update(watchList);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WatchListDetailsDto>(watchList);
        }

        public async Task<string> DeleteWatchListAsync(UpdateWatchListDto dto, int watchlistId)
        {
            var watchList = await _unitOfWork.WatchLists.FindAsync(w => w.Id == watchlistId && w.UserId == dto.UserId);
            if (watchList == null)
                throw new NotFoundException("Watchlist not found.");

            _unitOfWork.WatchLists.Delete(watchList);
            await _unitOfWork.SaveChangesAsync();

            return "Deleted Successfully";
        }

        public async Task<WatchListDetailsDto> AddMovieToWatchlistAsync(AddMovieToWatchlistDto dto, int watchlistId)
        {
            var watchList = await _unitOfWork.WatchLists.FindAsync(w => w.Id == watchlistId && w.UserId == dto.UserId);
            if (watchList == null)
                throw new NotFoundException("Watchlist not found.");

            var movie = await _unitOfWork.Movies.FindAsync(m => m.Id == dto.MovieId);
            if (movie == null)
                throw new NotFoundException("Movie not found.");

            if (watchList.Movies == null)
                watchList.Movies = new List<Movie>();

            if (watchList.Movies.Any(m => m.Id == dto.MovieId))
                throw new ServiceException((int)HttpStatusCode.BadRequest, "This movie is already in the watchlist.");

            watchList.Movies.Add(movie);

            _unitOfWork.WatchLists.Update(watchList);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WatchListDetailsDto>(watchList);
        }
    }
}
