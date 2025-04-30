using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.WatchListDtos;
using MoviesManagementSystem.Core.Interfaces.Services;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistsController : ControllerBase
    {
        private readonly IWatchListService _watchListService;

        public WatchlistsController(IWatchListService watchListService)
        {
            _watchListService = watchListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var watchlists = await _watchListService.GetAllWatchlistsAsync();
            return Ok(watchlists);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var watchlist = await _watchListService.GetWatchlistByIdAsync(id);
            return Ok(watchlist);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWatchListDto dto)
        {
            var created = await _watchListService.CreateWatchListAsync(dto, dto.UserId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWatchListDto dto)
        {
            var updated = await _watchListService.UpdateWatchListAsync(dto, id);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromBody] UpdateWatchListDto dto)
        {
            var message = await _watchListService.DeleteWatchListAsync(dto, id);
            return Ok(new { message });
        }

        [HttpPost("{id:int}/movies")]
        public async Task<IActionResult> AddMovie(int id, [FromBody] AddMovieToWatchlistDto dto)
        {
            var result = await _watchListService.AddMovieToWatchlistAsync(dto, id);
            return Ok(result);
        }
    }
}
