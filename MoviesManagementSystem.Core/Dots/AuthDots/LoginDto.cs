using System.ComponentModel.DataAnnotations;

namespace MoviesManagementSystem.Core.Dots.AuthDots
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}