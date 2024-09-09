using MoviesManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.EF.Models
{
    public class MovieReview
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
