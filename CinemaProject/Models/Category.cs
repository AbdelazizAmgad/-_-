using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Category
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Movie>? Movies { get; set; } 
    }
}
