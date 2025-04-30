namespace MoviesManagementSystem.Core.Dots.ReviewDtos
{
    public class ReviewDetailsDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public DateTime Date { get; set; }
    }
}
