using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.Movie;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Models;
using System.Net;

namespace MoviesManagementSystem.Core.Services
{
    public class MovieService(IUnitOfWork _unitOfWork, IMapper _mapper)
        : IMovieService
    {
        private List<string> _PhotoAllowedExtensions = new List<string> { ".jpg", ".png" };
        private List<string> _VideoAllowedExtensions = new List<string> { ".mp4", ".m4a", ".mkv", ".webm", ".mov", ".avi" };
        private long _PhotoMaxAllowedSize = 10485760;
        private long _videoMaxAllowedSize = 524288000;
            public async Task<IEnumerable<MovieDetailsDto>> GetAllMovies()
            {
                var movies = await _unitOfWork.Movies.FindAllWithIncludesAsync(new[] { "Reviews", "Rates", "LikedMovies", "Genres" });
                return _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            }
        public async Task<MovieDetailsDto> GetMovie(int id)
        {
            var movie = await _unitOfWork.Movies.FindAsync(m => m.Id == id, new[] { "Genres" });
            if (movie == null) throw new NotFoundException($"Movie With id :{id} was not found");
            return _mapper.Map<MovieDetailsDto>(movie);
        }
        public async Task<MovieDetailsDto> AddMovie([FromForm] CreateMovieDto dto)
        {
            var existingMovie = await _unitOfWork.Movies.FindAsync(m => m.Title == dto.Title);
            if (existingMovie is not null)
                throw new ServiceException((int)HttpStatusCode.BadRequest, "Movie with the same name already exists");

            var genres = new List<Genre>();
            foreach (var id in dto.GenreIds)
            {
                var existingGenre = await _unitOfWork.Genres.GetByIdAsync(id);
                if (existingGenre == null)
                    throw new NotFoundException("Genre Not Found");

                genres.Add(existingGenre);
            }

            var movie = _mapper.Map<Movie>(dto);
            movie.Genres = genres;

            if (dto.Poster is not null)
            {
                var extension = Path.GetExtension(dto.Poster.FileName).ToLower();
                if (!_PhotoAllowedExtensions.Contains(extension))
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Only .jpg and .png are allowed");

                if (dto.Poster.Length > _PhotoMaxAllowedSize)
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Max allowed size is 10MB");

                var imageName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/posters");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Poster.CopyToAsync(stream);
                }

                movie.PosterUrl = $"/images/posters/{imageName}";
            }
            if (dto.Video is not null)
            {
                var extension = Path.GetExtension(dto.Video.FileName).ToLower();
                if (!_VideoAllowedExtensions.Contains(extension))
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Only are allowed");

                if (dto.Video.Length > _videoMaxAllowedSize)
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Max allowed size is 500MB");

                var videoName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, videoName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Video.CopyToAsync(stream);
                }

                movie.VideoUrl = $"/videos/{videoName}";
            }

            await _unitOfWork.Movies.AddAsync(movie);
            await _unitOfWork.SaveChangesAsync();

            return  _mapper.Map<MovieDetailsDto>(movie);
        }
        public async Task<MovieDetailsDto> EditMovie([FromForm] EditMovie dto, int MovieID)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(MovieID);
            if (movie == null) throw new NotFoundException("Movie Not Found");

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.IsFree = dto.IsFree;

            if (dto.Poster is not null)
            {
                if (!string.IsNullOrEmpty(movie.PosterUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/posters/{movie.PosterUrl}");

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                var extension = Path.GetExtension(dto.Poster.FileName).ToLower();
                if (!_PhotoAllowedExtensions.Contains(extension))
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Only .jpg and .png are allowed");

                if (dto.Poster.Length > _PhotoMaxAllowedSize)
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Max allowed size is 10MB");

                var imageName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/posters");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Poster.CopyToAsync(stream);
                }

                movie.PosterUrl = $"/images/posters/{imageName}";
            }
            if (dto.Video is not null)
            {
                if (!string.IsNullOrEmpty(movie.VideoUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/videos/{movie.VideoUrl}");

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                var extension = Path.GetExtension(dto.Video.FileName).ToLower();
                if (!_VideoAllowedExtensions.Contains(extension))
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Only are allowed");

                if (dto.Video.Length > _videoMaxAllowedSize)
                    throw new ServiceException((int)HttpStatusCode.BadRequest, "Max allowed size is 500MB");

                var videoName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, videoName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Video.CopyToAsync(stream);
                }

                movie.VideoUrl = $"/videos/{videoName}";
            }

            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MovieDetailsDto>(movie);
        }

        public async Task<string> GetMovieVideo(int MovieId, string UserId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(MovieId);
            if (movie is null) throw new NotFoundException("Movie Not Found");

            if (movie.IsFree == false)
            {
                var payment = await _unitOfWork.Payments.FindAsync(b => b.MovieId == MovieId && b.UserId == UserId);
                if (payment == null) throw new ServiceException((int)HttpStatusCode.BadRequest, "You Shoud Pay first");
                else return movie.VideoUrl;
            }
            return movie.VideoUrl;
        }
        public async Task<string> DeleteMovie(int MovieID)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(MovieID);
            if (movie is null) throw new NotFoundException("Movie Not Found");

            var result = _unitOfWork.Movies.Delete(movie);
            await _unitOfWork.SaveChangesAsync();

            return "Movie deleted successfully";
        }
    }
}
