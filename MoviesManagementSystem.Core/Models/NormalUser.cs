using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Core.Models
{
    public class NormalUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
