using System.ComponentModel.DataAnnotations;

namespace CinemaProject.ViewModels
{
    public class VerifyOtpVM
    {
        public string UserId { get; set; } = string.Empty;
        [Required] public string OTP { get; set; } = string.Empty;
    }
}