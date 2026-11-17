using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Return list of users from database
            return View();
        }

        [HttpPost]
        public IActionResult Lock(string userId)
        {
            // TODO: Lock user logic
            TempData["SuccessMessage"] = "User has been locked.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Unlock(string userId)
        {
            // TODO: Unlock user logic
            TempData["SuccessMessage"] = "User has been unlocked.";
            return RedirectToAction("Index");
        }
    }
}