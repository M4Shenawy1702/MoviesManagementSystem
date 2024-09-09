using Microsoft.AspNetCore.Identity;
using MoviesManagementSystem.Core.Models;
using System.Numerics;

namespace MoviesManagementSystem.EF.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfileImg { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirtheDate { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public NormalUser NormalUser { get; set; }
        public SuperAdmin SuperAdmin { get; set; }
        public Admin Admin { get; set; }
        public MovieReview MovieReviews { get; set; }

    }
    public enum Gender
    {
        Male,
        Female
    }
}