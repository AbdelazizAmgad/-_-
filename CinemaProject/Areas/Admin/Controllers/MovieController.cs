using CinemaProject.Models;
using CinemaProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly Repository<Movie> _movieRepo;
        private readonly Repository<Category> _categoryRepo;
        private readonly Repository<Cinema> _cinemaRepo;
        private readonly Repository<Actor> _actorRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(
            Repository<Movie> movieRepo,
            Repository<Category> categoryRepo,
            Repository<Cinema> cinemaRepo,
            Repository<Actor> actorRepo,
            IWebHostEnvironment webHostEnvironment)
        {
            _movieRepo = movieRepo;
            _categoryRepo = categoryRepo;
            _cinemaRepo = cinemaRepo;
            _actorRepo = actorRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepo.GetQueryable()
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToListAsync();
            return View(movies);
        }

        private async Task PopulateDropDowns(int? categoryId = null, int? cinemaId = null, List<int>? selectedActors = null)
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", categoryId);
            ViewBag.CinemaId = new SelectList(await _cinemaRepo.GetAllAsync(), "Id", "Name", cinemaId);
            ViewBag.Actors = await _actorRepo.GetAllAsync();
            ViewBag.SelectedActors = selectedActors ?? new List<int>();
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile MainImg, List<IFormFile> SubImages, List<int> SelectedActors)
        {
            ModelState.Remove("MainImg");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("Actors");
            ModelState.Remove("SubImages");

            if (ModelState.IsValid)
            {
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

                movie.Actors = new List<Actor>();
                if (SelectedActors != null && SelectedActors.Count > 0)
                {
                    foreach (var actorId in SelectedActors)
                    {
                        var actor = await _actorRepo.GetByIdAsync(actorId);
                        if (actor != null) movie.Actors.Add(actor);
                    }
                }

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

                await _movieRepo.AddAsync(movie);
                await _movieRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(movie.CategoryId, movie.CinemaId, SelectedActors);
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _movieRepo.GetQueryable()
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var selectedActors = movie.Actors.Select(a => a.Id).ToList();
            await PopulateDropDowns(movie.CategoryId, movie.CinemaId, selectedActors);
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
                var existingMovie = await _movieRepo.GetQueryable()
                    .Include(m => m.Actors)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingMovie == null) return NotFound();

                existingMovie.Name = movie.Name;
                existingMovie.Description = movie.Description;
                existingMovie.Price = movie.Price;
                existingMovie.Status = movie.Status;
                existingMovie.IsHot = movie.IsHot; // ضفنا الـ Hot هنا
                existingMovie.DateTime = movie.DateTime;
                existingMovie.CategoryId = movie.CategoryId;
                existingMovie.CinemaId = movie.CinemaId;

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

                existingMovie.Actors.Clear();
                if (SelectedActors != null && SelectedActors.Count > 0)
                {
                    foreach (var actorId in SelectedActors)
                    {
                        var actor = await _actorRepo.GetByIdAsync(actorId);
                        if (actor != null) existingMovie.Actors.Add(actor);
                    }
                }

                _movieRepo.Update(existingMovie);
                await _movieRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(movie.CategoryId, movie.CinemaId, SelectedActors);
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if (movie != null)
            {
                _movieRepo.Delete(movie);
                await _movieRepo.SaveChangesAsync();
                return Json(new { success = true, message = "Movie deleted successfully." });
            }
            return Json(new { success = false, message = "Error while deleting." });
        }
    }
}