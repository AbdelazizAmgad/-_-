using CinemaProject.Data;
using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // صفحة العرض
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToListAsync();
            return View(movies);
        }

        // صفحة التفاصيل
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.Actors)
                .Include(m => m.SubImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            // بنجيب كل الكراسي المحجوزة للفيلم ده ونبعتهم للـ View
            var bookedSeats = await _context.Bookings
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

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        // DTO class to receive booking data from the client
        public class BookingRequest
        {
            public int MovieId { get; set; }
            public string Seats { get; set; } = string.Empty;
        }
    }
}