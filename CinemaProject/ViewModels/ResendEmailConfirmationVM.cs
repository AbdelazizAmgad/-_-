using System.ComponentModel.DataAnnotations;

namespace CinemaProject.ViewModels
{
    public class ResendEmailConfirmationVM
    {
        [Required] public string UserNameOrEmail { get; set; } = string.Empty;
    }
}