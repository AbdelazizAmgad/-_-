using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CinemaProject.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(1,1000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public bool Status { get; set; } 
        public DateTime DateTime { get; set; }
        [Required]
        public string MainImg { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public ICollection<Actor>? Actors { get; set; }
        public ICollection<MovieSubImage>? SubImages { get; set; }
        public bool IsHot { get; set; }

    }
}
