using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.RateDtos;
using MoviesManagementSystem.Core.IServices;

namespace MoviesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController(IRateService rateService)
        : ControllerBase
    {
        private readonly IRateService _rateService = rateService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllRatesForMovie(int id)
        {
            var result = await _rateService.GetAllRatesForMovie(id);
            return Ok(result);
        }

        [HttpGet("movie/{movieId}/user/{userId}")]
        public async Task<IActionResult> GetRateByUserAndMovie(int movieId, string userId)
        {
            var result = await _rateService.GetRateByUserAndMovie(movieId, userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] RateDto dto)
        {
            var result = await _rateService.AddRateMovie(dto);
            return CreatedAtAction(nameof(GetRateByUserAndMovie), new { movieId = dto.MovieId, userId = dto.UserId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> EditRate([FromBody] RateDto dto)
        {
            var result = await _rateService.EditRateMovie(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(int id)
        {
            await _rateService.DeleteRateAsync(id);
            return NoContent();
        }
    }
}
