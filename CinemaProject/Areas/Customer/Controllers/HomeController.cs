using CinemaProject.Models;
using CinemaProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly Repository<Movie> _movieRepo;
        private readonly Repository<Booking> _bookingRepo;

        public HomeController(Repository<Movie> movieRepo, Repository<Booking> bookingRepo)
        {
            _movieRepo = movieRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<IActionResult> Index()
        {
            // بنستخدم GetQueryable عشان نقدر نعمل Include
            var movies = await _movieRepo.GetQueryable()
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieRepo.GetQueryable()
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.Actors)
                .Include(m => m.SubImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var bookedSeats = await _bookingRepo.GetQueryable()
                .Where(b => b.MovieId == id)
                .Select(b => b.SeatNumbers)
                .ToListAsync();

            ViewBag.BookedSeats = string.Join(",", bookedSeats);
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBooking([FromBody] BookingRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Seats))
                return BadRequest("Invalid booking data.");

            var booking = new Booking
            {
                MovieId = request.MovieId,
                SeatNumbers = request.Seats,
                BookingDate = DateTime.Now
            };

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return Ok(new { success = true });
        }

        public class BookingRequest
        {
            public int MovieId { get; set; }
            public string Seats { get; set; } = string.Empty;
        }
    }
}