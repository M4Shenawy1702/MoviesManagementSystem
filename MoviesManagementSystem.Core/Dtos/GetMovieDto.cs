using Microsoft.AspNetCore.Http;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dtos
{
    public class GetMovieDto
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Poster { get; set; }
        public bool IsFree { get; set; }
        public int Likes { get; set; }
        public double AverageRating { get; set; }
        public int CategoryId { get; set; }

    }
}
