using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters;
namespace MoviesManagementSystem.Core.Dots

{
    public class RegisterDto
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(256)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public  DateTime BirtheDate { get; set; }
        public IFormFile ProfileImg { get; set; }

        //public Role Role { get; set; }

    }

    //public enum Role
    //{
    //    NormalUser,
    //    Admin,
    //    SuperAdmin
    //}
}