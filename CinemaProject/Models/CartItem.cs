namespace CinemaProject.Models
{
    public class CartItem
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public int ShowtimeId { get; set; }
        public string Showtime { get; set; }
        public List<int> SeatNumbers { get; set; } = new List<int>();
        public decimal PricePerSeat { get; set; }
        public decimal TotalPrice => PricePerSeat * SeatNumbers.Count;
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}