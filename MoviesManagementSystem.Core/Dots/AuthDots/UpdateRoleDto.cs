namespace MoviesManagementSystem.Core.Dots.AuthDots
{
    public class UpdateRoleModel
    {
        public required string OldRoleName { get; set; }
        public required string NewRoleName { get; set; }
    }
}