using CinemaProject.Data;
using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToListAsync();
            return View(movies);
        }

        // دالة مساعدة عشان نعبي القوائم المنسدلة والممثلين
        private void PopulateDropDowns(int? categoryId = null, int? cinemaId = null, List<int>? selectedActors = null)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", categoryId);
            ViewBag.CinemaId = new SelectList(_context.Cinemas, "Id", "Name", cinemaId);
            ViewBag.Actors = _context.Actors.ToList();
            ViewBag.SelectedActors = selectedActors ?? new List<int>();
        }

        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile MainImg, List<IFormFile> SubImages, List<int> SelectedActors)
        {
            // شيلنا التحقق من الحقول اللي بنملأها بإيدينا
            ModelState.Remove("MainImg");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("Actors");
            ModelState.Remove("SubImages");

            if (ModelState.IsValid)
            {
                // 1. حفظ الصورة الرئيسية
                if (MainImg != null && MainImg.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/movies");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await MainImg.CopyToAsync(stream);
                    }
                    movie.MainImg = "/images/movies/" + fileName;
                }

                // 2. ربط الممثلين
                movie.Actors = new List<Actor>();
                if (SelectedActors != null && SelectedActors.Count > 0)
                {
                    foreach (var actorId in SelectedActors)
                    {
                        var actor = await _context.Actors.FindAsync(actorId);
                        if (actor != null) movie.Actors.Add(actor);
                    }
                }

                // 3. حفظ الصور الفرعية
                movie.SubImages = new List<MovieSubImage>();
                if (SubImages != null && SubImages.Count > 0)
                {
                    string subFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/movies/sub");
                    if (!Directory.Exists(subFolder)) Directory.CreateDirectory(subFolder);

                    foreach (var file in SubImages)
                    {
                        if (file.Length > 0)
                        {
                            string subName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string subPath = Path.Combine(subFolder, subName);
                            using (var stream = new FileStream(subPath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            movie.SubImages.Add(new MovieSubImage { ImgUrl = "/images/movies/sub/" + subName });
                        }
                    }
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(movie.CategoryId, movie.CinemaId, SelectedActors);
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var selectedActors = movie.Actors.Select(a => a.Id).ToList();
            PopulateDropDowns(movie.CategoryId, movie.CinemaId, selectedActors);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? MainImg, List<IFormFile> SubImages, List<int> SelectedActors)
        {
            if (id != movie.Id) return NotFound();

            ModelState.Remove("MainImg");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("Actors");
            ModelState.Remove("SubImages");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await _context.Movies
                        .Include(m => m.Actors)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingMovie == null) return NotFound();

                    existingMovie.Name = movie.Name;
                    existingMovie.Description = movie.Description;
                    existingMovie.Price = movie.Price;
                    existingMovie.Status = movie.Status;
                    existingMovie.DateTime = movie.DateTime;
                    existingMovie.CategoryId = movie.CategoryId;
                    existingMovie.CinemaId = movie.CinemaId;

                    // تحديث الصورة الرئيسية لو تم رفع واحدة جديدة
                    if (MainImg != null && MainImg.Length > 0)
                    {
                        string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/movies");
                        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                        string filePath = Path.Combine(folder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await MainImg.CopyToAsync(stream);
                        }
                        existingMovie.MainImg = "/images/movies/" + fileName;
                    }

                    // تحديث الممثلين
                    existingMovie.Actors.Clear();
                    if (SelectedActors != null && SelectedActors.Count > 0)
                    {
                        foreach (var actorId in SelectedActors)
                        {
                            var actor = await _context.Actors.FindAsync(actorId);
                            if (actor != null) existingMovie.Actors.Add(actor);
                        }
                    }

                    _context.Update(existingMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Movies.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(movie.CategoryId, movie.CinemaId, SelectedActors);
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Movie deleted successfully." });
            }
            return Json(new { success = false, message = "Error while deleting." });
        }
    }
}