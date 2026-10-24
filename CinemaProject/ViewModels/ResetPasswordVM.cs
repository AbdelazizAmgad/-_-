using System.ComponentModel.DataAnnotations;

namespace CinemaProject.ViewModels
{
    public class ResetPasswordVM
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)] public string NewPassword { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare("NewPassword")] public string ConfirmPassword { get; set; } = string.Empty;
    }
}