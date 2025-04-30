using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dots.WatchListDtos
{
    public class UpdateWatchListDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }

    }
}
