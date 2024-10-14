using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Models
{
    public class Movie
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[]? Poster { get; set; }
        public string? Video { get; set; }
        public bool IsFree { get; set; }
        public int Likes { get; set; }
        public double AverageRating => MovieReviews?.Count > 0 ? MovieReviews.Average(r => r.Rating) : 0;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<MovieReview>? MovieReviews { get; set; }


    }
}
