namespace CinemaProject.Models
{
    public class ApplicationUserOtp
    {
        public int Id { get; set; }
        public string OTP { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public bool IsValid { get; set; } = true;
        public DateTime ValidTo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUserOtp() { }

        public ApplicationUserOtp(string otp, string userId)
        {
            OTP = otp;
            ApplicationUserId = userId;
            ValidTo = DateTime.UtcNow.AddMinutes(10);
        }
    }
}