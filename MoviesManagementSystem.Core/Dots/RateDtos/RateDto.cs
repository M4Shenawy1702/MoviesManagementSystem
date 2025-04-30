using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dots.RateDtos
{
    public class RateDto
    {
        public int Score { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
    }
}
