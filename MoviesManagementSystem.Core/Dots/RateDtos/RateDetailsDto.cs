namespace MoviesManagementSystem.Core.Dots.RateDtos
{
    public class RateDetailsDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; } = null!;

    }
}
