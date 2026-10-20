using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }
        public string SeatNumbers { get; set; } = string.Empty; // هنخزن الكراسي كـ "A1,A2,B5"
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}