using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.Movie;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IMovieService
    {
        Task<MovieDetailsDto> AddMovie([FromForm] CreateMovieDto dto);
        Task<string> DeleteMovie(int MovieID);
        Task<MovieDetailsDto> EditMovie([FromForm] EditMovie dto, int MovieID);
        Task<IEnumerable<MovieDetailsDto>> GetAllMovies();
        Task<MovieDetailsDto> GetMovie(int id);
        Task<string> GetMovieVideo(int MovieId, string UserId);
    }
}