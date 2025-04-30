namespace MoviesManagementSystem.Core.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public bool IsFree { get; set; }
        public int Likes { get; set; }
        public double AverageRating => Rates?.Count > 0 ? Rates.Average(r => r.Score) : 0;
        public List<Rate> Rates { get; set; } = [];
        public ICollection<Genre> Genres { get; set; } = [];
        public ICollection<WatchList> WatchLists { get; set; } = [];
        public List<Review>? Reviews { get; set; } = [];
        public List<Like>? LikedMovies { get; set; } = [];


    }
}
