using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.ReviewDtos;
using MoviesManagementSystem.Core.IServices;

namespace MoviesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpPost]
        public async Task<ActionResult<ReviewDetailsDto>> CreatReview([FromForm] ReviewDto dto)
        {
            return Ok(await _reviewService.AddReview(dto));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ReviewDetailsDto>> CreatReview([FromForm] ReviewDto dto, int id)
        {
            return Ok(await _reviewService.EditReview(dto, id));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDetailsDto>> GetReview(int id)
        {
            return Ok(await _reviewService.GetReviewsForMovie(id));
        }
        [HttpGet("get-all-for-movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ReviewDetailsDto>>> GetAllReviewforMovie(int movieId)
        {
            return Ok(await _reviewService.GetAllReviewsForMovie(movieId));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<string>> DeleteReview(int id)
        {
            return await _reviewService.DeleteReview(id);
        }
    }
}
