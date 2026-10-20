using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Img { get; set; } = string.Empty;
        public ICollection<Movie>? Movies { get; set; }
    }
}
