using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Core.Dots.WatchListDtos
{
    public class AddMovieToWatchlistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
