using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.IServices;

namespace MoviesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController(IGenreService genreService)
        : ControllerBase
    {
        private readonly IGenreService _genreService = genreService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDatailsDto>>> GetAllGenres()
        {
            return Ok(await _genreService.GetAllGenres());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDatailsDto>> GetGenre(int id)
        {
            var genre = await _genreService.GetGenre(id);
            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        [HttpPost]
        public async Task<ActionResult<GenreDatailsDto>> AddGenre([FromForm] AddGenreDto dto)
        {
            var genre = await _genreService.AddGenre(dto);
            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenreDatailsDto>> EditGenre(int id, [FromForm] AddGenreDto dto)
        {
            var genre = await _genreService.EditGenre(dto, id);
            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var result = await _genreService.DeleteGenre(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
