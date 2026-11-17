using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProfile(string FullName, string PhoneNumber)
        {
            // TODO: Update user data in database
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangePassword(string CurrentPassword, string NewPassword)
        {
            // TODO: Change password logic
            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Index");
        }
    }
}