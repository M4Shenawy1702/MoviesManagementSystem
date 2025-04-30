namespace MoviesManagementSystem.Core.Dots.Movie
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Poster { get; set; }
        public string? Video { get; set; }
        public bool IsFree { get; set; }
        public int Likes { get; set; }
        public double AverageRating { get; set; }
        public ICollection<String> GenresName { get; set; } = [];
    }
}
