using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Models
{
    public class WatchList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [JsonIgnore]
        public ICollection<Movie> Movies { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
