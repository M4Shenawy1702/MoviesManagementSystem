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
    public class AddMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Poster { get; set; }
        public IFormFile? Video { get; set; }
        public bool IsFree { get; set; }
        public int CategoryId { get; set; }
    }
}
