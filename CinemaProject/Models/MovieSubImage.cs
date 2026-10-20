namespace CinemaProject.Models
{
    public class MovieSubImage
    {
        public int Id { get; set; }
        
        public string ImgUrl { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
