using MoviesManagementSystem.Core.Dots.Movie;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dots.WatchListDtos
{
    public class WatchListDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<MovieDetailsDto> Movies { get; set; } = [];
        public DateTime AddedAt { get; set; }
    }
}
