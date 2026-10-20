using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinemaProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // تحويل تلقائي لواجهة الكستمر
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
