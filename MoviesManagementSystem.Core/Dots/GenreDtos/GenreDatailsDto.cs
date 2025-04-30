namespace MoviesManagementSystem.Core.Dtos
{
    public class GenreDatailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<string> MoviesNames { get; set; }
    }
}
