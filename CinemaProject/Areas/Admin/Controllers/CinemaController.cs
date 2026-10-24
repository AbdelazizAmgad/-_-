using CinemaProject.Models;
using CinemaProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly Repository<Cinema> _cinemaRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Inject Repository & Environment
        public CinemaController(Repository<Cinema> cinemaRepo, IWebHostEnvironment webHostEnvironment)
        {
            _cinemaRepo = cinemaRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var cinemas = await _cinemaRepo.GetAllAsync();
            return View(cinemas);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile file)
        {
            ModelState.Remove("Img");
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/cinemas");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    cinema.Img = "/images/cinemas/" + fileName;
                }
                await _cinemaRepo.AddAsync(cinema);
                await _cinemaRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cinema = await _cinemaRepo.GetByIdAsync(id.Value);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cinema cinema, IFormFile? file)
        {
            if (id != cinema.Id) return NotFound();
            ModelState.Remove("Img");
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/cinemas");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    cinema.Img = "/images/cinemas/" + fileName;
                }
                _cinemaRepo.Update(cinema);
                await _cinemaRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemaRepo.GetByIdAsync(id);
            if (cinema != null)
            {
                _cinemaRepo.Delete(cinema);
                await _cinemaRepo.SaveChangesAsync();
                return Json(new { success = true, message = "Cinema deleted successfully." });
            }
            return Json(new { success = false, message = "Error while deleting." });
        }
    }
}