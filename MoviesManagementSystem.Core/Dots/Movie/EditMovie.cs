using Microsoft.AspNetCore.Http;

namespace MoviesManagementSystem.Core.Dots.Movie
{
    public class EditMovie
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public IFormFile Poster { get; set; }
        public IFormFile Video { get; set; }
        public bool IsFree { get; set; }

    }
}
