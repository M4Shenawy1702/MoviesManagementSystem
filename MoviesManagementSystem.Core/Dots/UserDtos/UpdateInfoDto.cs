using Microsoft.AspNetCore.Http;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dots.UserDtos
{
    public class UpdateInfoDto
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(128)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirtheDate { get; set; }
        public IFormFile ProfileImg { get; set; }
    }
}
