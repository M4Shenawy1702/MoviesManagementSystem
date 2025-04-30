using MoviesManagementSystem.EF.Models;
using System.Text.Json.Serialization;

namespace MoviesManagementSystem.Core.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
    }
}
