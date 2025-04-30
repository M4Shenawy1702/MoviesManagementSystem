using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Core.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
