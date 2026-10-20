using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Img { get; set; } = string.Empty;

        public ICollection<Movie>? Movies { get; set; }

    }
}
