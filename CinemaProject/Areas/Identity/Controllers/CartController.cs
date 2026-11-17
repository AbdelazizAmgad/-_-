using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CinemaProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(json);
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(CartSessionKey, json);
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        public IActionResult AddToCart(int movieId, string movieTitle, int showtimeId, string showtime, List<int> seats, decimal pricePerSeat)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(x => x.ShowtimeId == showtimeId);

            if (existing != null)
            {
                existing.SeatNumbers.AddRange(seats.Except(existing.SeatNumbers));
            }
            else
            {
                cart.Add(new CartItem
                {
                    MovieId = movieId,
                    MovieTitle = movieTitle,
                    ShowtimeId = showtimeId,
                    Showtime = showtime,
                    SeatNumbers = seats,
                    PricePerSeat = pricePerSeat
                });
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction(nameof(Index));

            var options = new SessionCreateOptions
            {
                LineItems = cart.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.PricePerSeat * 100),
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions { Name = $"{item.MovieTitle} - {item.Showtime}" }
                    },
                    Quantity = item.SeatNumbers.Count
                }).ToList(),
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Cart", null, Request.Scheme),
                CancelUrl = Url.Action("Index", "Cart", null, Request.Scheme)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return Redirect(session.Url);
        }

        public IActionResult Success()
        {
            HttpContext.Session.Remove(CartSessionKey);
            TempData["SuccessMessage"] = "Payment successful! Your booking has been confirmed.";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult MyBookings()
        {
            return View();
        }
    }
}