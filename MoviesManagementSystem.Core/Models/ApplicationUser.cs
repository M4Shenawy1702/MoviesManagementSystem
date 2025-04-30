using Microsoft.AspNetCore.Identity;
using MoviesManagementSystem.Core.Models;

namespace MoviesManagementSystem.EF.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfileImgPath { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public int? Age { get; set; }
        public NormalUser NormalUser { get; set; }
        public SuperAdmin SuperAdmin { get; set; }
        public Admin Admin { get; set; }
        public Review Reviews { get; set; }
        List<WatchList> WatchLists { get; set; }
        public List<Like>? LikedMovies { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}