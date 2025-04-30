namespace MoviesManagementSystem.Core.Dots.ReviewDtos
{
    public class ReviewDto
    {
        public required string Content { get; set; }
        public required string UserId { get; set; }
        public required int MovieId { get; set; }
    }
}
