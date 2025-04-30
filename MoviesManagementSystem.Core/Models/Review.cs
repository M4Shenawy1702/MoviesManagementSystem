using MoviesManagementSystem.EF.Models;
using System.Text.Json.Serialization;

namespace MoviesManagementSystem.Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        [JsonIgnore]
        public ApplicationUser User { get; set; } 
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
        public DateTime Date { get; set; }
    }
}
