using System.ComponentModel.DataAnnotations;

namespace MoviesManagementSystem.Core.Dots
{
    public class TokenRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}