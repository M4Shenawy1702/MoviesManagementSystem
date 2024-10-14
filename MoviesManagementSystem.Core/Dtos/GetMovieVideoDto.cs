using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dtos
{
    public class GetMovieVideoDto
    {
        public IFormFile Video { get; set; }
    }
}
