using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dtos;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IGenreService
    {
        Task<GenreDatailsDto> AddGenre([FromForm] AddGenreDto Dto);
        Task<string> DeleteGenre(int GenreID);
        Task<GenreDatailsDto> EditGenre([FromForm] AddGenreDto Dto, int GenreID);
        Task<IEnumerable<GenreDatailsDto>> GetAllGenres();
        Task<GenreDatailsDto> GetGenre(int GenreId);
    }
}
