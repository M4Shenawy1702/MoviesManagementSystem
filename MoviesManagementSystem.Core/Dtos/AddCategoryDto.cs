using MoviesManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dtos
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
