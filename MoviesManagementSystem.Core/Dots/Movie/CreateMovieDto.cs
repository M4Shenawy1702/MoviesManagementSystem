using Microsoft.AspNetCore.Http;

namespace MoviesManagementSystem.Core.Dots.Movie
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Poster { get; set; }
        public IFormFile Video { get; set; }
        public bool IsFree { get; set; }
        public List<int> GenreIds { get; set; }
    }
}
