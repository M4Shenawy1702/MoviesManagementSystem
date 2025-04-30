using System.Text.Json.Serialization;

namespace MoviesManagementSystem.Core.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Movie> Movies { get; set; }
    }
}
