using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dots.WatchListDtos
{
    public class CreateWatchListDto
    {
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
