using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dots.UserDtos
{
    public class UserInfoDto
    {
        public string? Id { get; set; }
        public string? ProfileImgPath { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public int? Age { get; set; }
        public IEnumerable<string> RolesName { get; set; } = new List<string>();
    }
}
