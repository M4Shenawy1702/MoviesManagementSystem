using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Models;

namespace MoviesManagementSystem.Core.Services
{

    public class GenreService(IUnitOfWork _unitOfWork, IMapper _mapper) : IGenreService
    {
        public async Task<IEnumerable<GenreDatailsDto>> GetAllGenres()
        {
            var genres = await _unitOfWork.Genres.GetAllAsync();

            return _mapper.Map<IEnumerable<GenreDatailsDto>>(genres);
        }
        public async Task<GenreDatailsDto> GetGenre(int GenreId)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(GenreId);

            return _mapper.Map<GenreDatailsDto>(genre);
        }
        public async Task<GenreDatailsDto> AddGenre([FromForm] AddGenreDto Dto)
        {
            var genre = _mapper.Map<Genre>(Dto);

            var result = await _unitOfWork.Genres.AddAsync(genre);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GenreDatailsDto>(genre);
        }

        public async Task<GenreDatailsDto> EditGenre([FromForm] AddGenreDto Dto, int GenreID)
        {
            var genre = _mapper.Map<Genre>(Dto);
            var result = await _unitOfWork.Genres.GetByIdAsync(genre);
            if (genre == null) throw new NotFoundException("Genre not found");

            genre.Name = Dto.Name;
            genre.Description = Dto.Description;

            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GenreDatailsDto>(genre);
        }

        public async Task<string> DeleteGenre(int GenreID)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(GenreID);
            if (genre == null) throw new NotFoundException("Genre not found");

            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.SaveChangesAsync();

            return "Genre deleted successfully.";
        }
    }
}
