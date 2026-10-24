using CinemaProject.Models;
using CinemaProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        // حقن الريبو
        private readonly Repository<Category> _categoryRepo;

        public CategoryController(Repository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepo.AddAsync(category);
                await _categoryRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        // (باقي الدوال Edit و Delete بنفس الطريقة)
    }
}