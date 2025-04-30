using MoviesManagementSystem.EF.Models;
using System.Text.Json.Serialization;

namespace MoviesManagementSystem.Core.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public int Score { get; set; } 
        public DateTime Date { get; set; }
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        ApplicationUser User { get; set; }
    }
}
