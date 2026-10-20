using CinemaProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalMovies = await _context.Movies.CountAsync();
            ViewBag.TotalActors = await _context.Actors.CountAsync();
            ViewBag.TotalCinemas = await _context.Cinemas.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();
            return View();
        }
    }
}