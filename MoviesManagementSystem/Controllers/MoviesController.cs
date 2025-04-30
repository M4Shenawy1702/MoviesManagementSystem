using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.Movie;
using MoviesManagementSystem.Core.IServices;

namespace MoviesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController(IMovieService movieService)
        : ControllerBase
    {
        private readonly IMovieService _movieService = movieService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetAllMovies()
        {
            var Movies = await _movieService.GetAllMovies();
            return Ok(Movies);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsDto>> GetMovie(int id)
        {
            return Ok(await _movieService.GetMovie(id));
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromForm] CreateMovieDto dto)
        {
            return Ok(await _movieService.AddMovie(dto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDetailsDto>> EditMovie([FromForm] EditMovie dto, int id)
        {
            return Ok(await _movieService.EditMovie(dto, id));
        }

        [HttpGet("GetMovieVideo/{UserId}/{MovieId}")]
        public async Task<ActionResult<string>> GetMovieVideo(int MovieId, string UserId)
        {
            return Ok(await _movieService.GetMovieVideo(MovieId, UserId));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            return Ok(await _movieService.DeleteMovie(id));
        }
    }
}
