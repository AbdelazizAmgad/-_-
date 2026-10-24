using CinemaProject.Models;
using CinemaProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly Repository<Actor> _actorRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ActorController(Repository<Actor> actorRepo, IWebHostEnvironment webHostEnvironment)
        {
            _actorRepo = actorRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var actors = await _actorRepo.GetAllAsync();
            return View(actors);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Actor actor, IFormFile file)
        {
            ModelState.Remove("Img");
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/actors");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    actor.Img = "/images/actors/" + fileName;
                }
                await _actorRepo.AddAsync(actor);
                await _actorRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var actor = await _actorRepo.GetByIdAsync(id.Value);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Actor actor, IFormFile? file)
        {
            if (id != actor.Id) return NotFound();
            ModelState.Remove("Img");
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/actors");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    actor.Img = "/images/actors/" + fileName;
                }
                _actorRepo.Update(actor);
                await _actorRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorRepo.GetByIdAsync(id);
            if (actor != null)
            {
                _actorRepo.Delete(actor);
                await _actorRepo.SaveChangesAsync();
                return Json(new { success = true, message = "Actor deleted successfully." });
            }
            return Json(new { success = false, message = "Error while deleting." });
        }
    }
}